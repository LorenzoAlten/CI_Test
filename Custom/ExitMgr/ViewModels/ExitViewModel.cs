using AgilogDll;
using AgilogDll.DTO;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using Microsoft.AspNetCore.Mvc;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwAgilogDll.ViewModels;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExitMgr.ViewModels
{
    /*
     La discriminante fra lo stoccaggio e il semplice transito viene determinata con le seguenti regole:
         A.	Le UDC intere mono-referenza, di default, sono tutte destinate allo stoccaggio.
         B.	Quando in qualunque ingresso viene depositata un’UDC di prodotto finito, il sistema deve valutare se esiste un ordine di prelievo attivo per la stessa referenza, 
            per la quale non sia stato già riservato un numero di UDC sufficiente a coprire l’intera quantità richiesta. 
            In questo caso l’UDC deve essere rediretta in uscita:
            i.	Se la quantità d’ordine rimanente è multipla intera dell’UDC, l’UDC deve essere portato in uscita, dopo essere stato fasciato per la vendita ed etichettato con l’etichetta di vendita.
            ii.	Se la quantità d’ordine rimanente NON è multipla intera dell’UDC, l’UDC deve es-sere rediretta in Picking, senza essere fasciata né etichettata.
         C.	Quando un UDC di picking multi-referenza viene fatta entrare per la baia manuale, la HMI di Agilog deve proporre all’operatore la scelta fra stoccaggio o transito.
            In entrambi i casi, essendo l’UDC già destinata alla vendita, deve sempre essere fasciata per la vendita ed etichettata.
         D.	Tramite l’HMI di Agilog deve essere sempre possibile abilitare o disabilitare un flag che consenta di “fermare” le UDC in ingresso dalla baia automatica 
            per consentire all’operatore la scelta, una per una, fra stoccaggio e transito. In questo modo l’operatore ha sempre la possibilità di sovrascrivere il comportamento del sistema 
            in caso di necessità.
     */
    [Export(typeof(ExitViewModel))]
    public class ExitViewModel : Conductor<Screen>
    {
        #region Enum
        public enum EOperationType
        {
            ToWarehouse,
            OnlyWrap,
        }
        #endregion

        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly SqlConnection _conn;
        private AgilogDll.RunUtils _utils;
        private object _lockObj = new object();
        private string _wcsIdentity = null;
        private string _controllerIdentity = null;

        /// <summary>
        /// Tipi di telegrammi gestiti
        /// </summary>
        private readonly List<ETelegramTypes> ManagedTelegrams = new List<ETelegramTypes>() { ETelegramTypes.DREQ, ETelegramTypes.LCAP, ETelegramTypes.LPRE };

        /// <summary>
        /// Numeri delle baie gestite
        /// </summary>
        private readonly List<int> BaysNr;

        /// <summary>
        /// Lista delle baie gestite
        /// </summary>
        private List<HndBayBay> ManagedBays = new List<HndBayBay>();

        private int Controller;
        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private AgilogDll.UdcUdc _Udc;
        private bool _Presenza;
        private string _UDCBarcode = "";
        private string _Direction = "";        
        private bool _IsTransit = true;
        private bool _IsToWarehouse=false;
        private List<CompartementDTO> _compartmentDTO;
        

       


        public string _controller;
        /// <summary>
        /// Funzionamento manuale baia automatica
        /// Flag che consente di fermare le UDC in ingresso dalla baia automatica per consentire all’operatore la scelta fra stoccaggio e transito
        /// </summary>
        private bool _ManualOperation = false;
        private HndBayBay _CurrentBay;

        #endregion Members

        #region Properties

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public string SnackBarMessage
        {
            get { return _SnackBarMessage; }
            set
            {
                // Forzo cambio di valore
                _SnackBarMessage = string.Empty;
                NotifyOfPropertyChange(() => SnackBarMessage);

                _SnackBarMessage = value;
                NotifyOfPropertyChange(() => SnackBarMessage);
            }
        }

        public AgilogDll.UdcUdc Udc
        {
            get { return _Udc; }
            set
            {
                _Udc = value;
                NotifyOfPropertyChange(() => Udc);
                NotifyOfPropertyChange(() => CanConfirm);
                NotifyOfPropertyChange(() => CanToWarehouse);
                NotifyOfPropertyChange(() => CanTransit);
            }
        }

        public string UDCBarcode
        {
            get { return _UDCBarcode; }
            set
            {
                _UDCBarcode = value;
                NotifyOfPropertyChange(() => UDCBarcode);

                LoadUdcFromDb(_UDCBarcode);
                 
                //_UDCBarcode = "";
                //NotifyOfPropertyChange(() => UDCBarcode);
            }
        }
        public string Direction
        {
            get { return _Direction; }
            set
            {
                _Direction = value;
                NotifyOfPropertyChange(() => Direction);
             
            }
        }
        public bool IsTransit
        {
            get { return _IsTransit; }
            set
            {
                _IsTransit = value;
                NotifyOfPropertyChange(() => IsTransit);
            }
        }


        public bool IsToWarehouse
        {
            get { return _IsToWarehouse; }
            set
            {
                _IsToWarehouse = value;
                NotifyOfPropertyChange(() => IsToWarehouse);
            }
        }

        public List<CompartementDTO> CompartmentDTO
        {
            get { return _compartmentDTO; }
            set
            {
                _compartmentDTO = value;
                NotifyOfPropertyChange(() => CompartmentDTO);
            }
        }
        public bool WaitingUDC { get { return !Presenza; } }

        /// <summary>
        /// Classe altezza
        /// </summary>
        public WhsCfgHeightClasse HeightClass { get { return Udc?.HeightClass; } }
        /// <summary>
        /// Classe lunghezza
        /// </summary>
        public WhsCfgLengthClasse LengthClass { get { return Udc?.LengthClass; } }
        /// <summary>
        /// Classe peso
        /// </summary>
        public WhsCfgWeightClasse WeightClass { get { return Udc?.WeightClass; } }
        /// <summary>
        /// Classe larghezza
        /// </summary>
        public WhsCfgWidthClasse WidthClass { get { return Udc?.WidthClass; } }

        public bool UdcHasErrors
        {
            get
            {
                return Presenza

                        &&
                        !string.IsNullOrWhiteSpace(UDCBarcode) &&
                        Udc != null &&
                        ((HeightClass != null && HeightClass.HGT_Waste) ||
                        (LengthClass != null && LengthClass.LNG_Waste) ||
                        (WeightClass != null && WeightClass.WGH_Waste) ||
                        (WidthClass != null && WidthClass.WDT_Waste));

                return false;
            }
        }

        public bool Monoreference { get { return Udc != null && Udc.Compartments.Select(x => x.UCM_ITM_Code).Count() == 1; } }
        public bool Multireference { get { return Udc != null && Udc.Compartments.Select(x => x.UCM_ITM_Code).Count() > 1; } }

        /// <summary>
        /// Presenza fisica della cassetta sul modulo
        /// </summary>
        public bool Presenza
        {
            get { return _Presenza; }
            set
            {
                _Presenza = value;
                NotifyOfPropertyChange(() => Presenza);
                NotifyOfPropertyChange(() => UdcHasErrors);
                NotifyOfPropertyChange(() => CanConfirm);
                //NotifyOfPropertyChange(() => CanToWarehouse);
                //NotifyOfPropertyChange(() => CanTransit);
                NotifyOfPropertyChange(() => WaitingUDC);                
            }
        }

        public bool CanConfirm
        {
            get
            {
                return Presenza &&
                       (Udc != null) 
                       //&&
                       //(_CurrentBay != null) &&
                       //(_CurrentBay.BAY_SCT_Code == "M" || _ManualOperation)
                       //&&
                       //Monoreference
                       ;
            }
        }

        public bool CanToWarehouse
        {
            get
            {
                return Presenza &&
                       (Udc != null) &&
                       (_CurrentBay != null) &&
                       (_CurrentBay.BAY_SCT_Code == "M" || _ManualOperation)
                       //&&
                       //IsToWarehouse
                       //&&
                       //Multireference
                       ;
            }
        }

        public bool CanTransit
        {
            get
            {
                return Presenza &&
                       (Udc != null) &&
                       (_CurrentBay != null) &&
                       (_CurrentBay.BAY_SCT_Code == "M" || _ManualOperation) 
                       //&&
                       //IsTransit
                       //&&
                       //Multireference
                       ;
            }
        }

        #endregion Properties

        #region Constructor

        [ImportingConstructor]
        public ExitViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, List<int> baysNr, int controller = 0, bool reEntry = false)
        {
            DisplayName = Global.Instance.LangTl("Baia uscita");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));
            
            BaysNr = baysNr;
            Controller = controller;
            LoadMissionState();
            Global.Instance.OnTelegramManagement += Global_OnTelegramManagement;
        }


        #endregion Constructor

        #region ViewModel Override

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnTelegramManagement -= Global_OnTelegramManagement;
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }
       

        Views.ExitView View;

        protected override async void OnViewLoaded(object view)
        {
            View = (Views.ExitView)view;

            IsLoading = true;

            await LoadData();

            IsLoading = false;

            base.OnViewLoaded(view);
        }

        #endregion ViewModel Override

        #region Global Events

        private void Global_OnTelegramManagement(object sender, GenericEventArgs e)
        {
            try
            {
                int controller = (int)e.Arguments[0];
                string message = e.Arguments[1].ToString();

                

                var telegram = new Handling_Tel();
                telegram.ParseReceivedMessage(message, out var response);

                _wcsIdentity = telegram.WcsIdentity;
                _controllerIdentity = telegram.ControllerIdentity;

                if (!ManagedTelegrams.Contains(telegram.TelegramType)) return;


                
                if (ManagedBays.Any(bay => bay.HndBayModules.Any(mod => mod.BMO_MOD_Code == telegram.Position)))
                {
                    if (telegram.TelegramType == ETelegramTypes.DREQ)
                    {
                        ProcessDREQ(telegram);
                    }
                    else if (telegram.TelegramType == ETelegramTypes.LCAP)
                    {
                        ProcessLCAP(telegram);
                    }
                    else if (telegram.TelegramType == ETelegramTypes.LPRE)
                    {
                        ProcessLPRE(telegram);
                    }

                    if (ManagedTelegrams.Contains(telegram.TelegramType))
                    {
                        _eventAggregator.PublishOnUIThreadAsync(true);
                    }
                }
            }

            
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }
        }

        #endregion Global Events

        #region Private methods

        private void ProcessDREQ(TelegramBase telegram)
        {
            if (Presenza) return;

            Presenza = true;

            _CurrentBay = ManagedBays.FirstOrDefault(bay => bay.HndBayModules.Any(mod => mod.BMO_MOD_Code == telegram.Position));

            if (!string.IsNullOrEmpty(telegram.UDC_Barcode) &&
                !telegram.UDC_Barcode_Error &&
                telegram.UDC_Barcode_IsValid)
            {
                UDCBarcode = telegram.UDC_Barcode;
            }
        }

        private void ProcessLCAP(TelegramBase telegram)
        {



            var udcType = telegram.UDC_Type;
            var barcode = telegram.UDC_Barcode.EndsWith("A") ||
                          telegram.UDC_Barcode.EndsWith("C") ? telegram.UDC_Barcode.Substring(0, telegram.UDC_Barcode.Length - 1) : telegram.UDC_Barcode;

            //if (Presenza && HasUDC)
            //{
            //    if (UDC.UdcCode != barcode)
            //    {
            //        Global.Instance.Log($"UDC Mismatch! Logico={UDC.UdcCode} Fisico={barcode}", LogLevels.Event);
            //        Global.Error(_windowManager, Global.Instance.LangTl("UDC barcode mismatch! Data will be reloaded"));
            //    }
            //    else return;
            //}

            Presenza = true;
            IsLoading = true;
            //Bolero_Time = DateTime.Now;

            var mission = _utils.GetMissions((SqlConnection)Global.Instance.ConnGlobal, $"WHERE MIS_Id = {telegram.MissionID.SqlFormat()}").FirstOrDefault();



            // Se non trovo la missione oppure è una missione di evacuazione, esco
            if (mission == null)
            //if (mission == null || mission.EvacRequest != null)
            {
                IsLoading = false;
                Presenza = false;
                return;
            }


            //// Se ho incongruenza di tracking
            //if (barcode != mission.MIS_UDC_Code && udcType < 90) udcType = 98;

            UDCBarcode = barcode;

            IsLoading = false;

            ////_TrackingUDCCode = telegram.UDC_Barcode;

            ////switch (udcType)
            ////{
            ////    // incongruenza barcode
            ////    case 98:
            ////        if (_badUdc != barcode) break;

            ////        _badUdc = barcode;
            ////        CanConfirmItem = false;
            ////        CanReadUDC = false;
            ////        Global.Alert(_windowManager, Global.Instance.LangTl("Inconsistency between the barcode read and the data tracking."));
            ////        break;

            ////    // barcode illeggibile
            ////    case 99:
            ////        if (_badUdc != barcode) break;

            ////        _badUdc = barcode;
            ////        CanConfirmItem = false;
            ////        //CanReadUDC = true;
            ////        break;

            ////    default:
            ////        CanConfirmItem = true;
            ////        CanReadUDC = false;
            ////        _badUdc = null;
            ////        if (!DbUtils.ExecuteNonQuery($"EXECUTE [dbo].[msp_HND_BAY_ORDER_SetUDCBarcode] {BayNr}, {_TrackingUDCCode.SqlFormat()}", _conn, KeepOpen: true))
            ////        {
            ////            Global.Alert(_windowManager, Global.Instance.LangTl("An error occurred while trying to register the UDC on current bay"));
            ////            Presenza = false;
            ////            return;
            ////        }
            ////        SetUdcAsync(_TrackingUDCCode);
            ////        break;
            ////}

            /////* Invio il CMND per farmi dare il peso dell'UDC vuota */
            ////_WaitingUdcWeight = true;
            ////SendTelegramCMND();
        }

        private void ProcessLPRE(TelegramBase telegram)
        {
            RemovePresence();
        }

        private void RemovePresence()
        {
            Presenza = false;
            UDCBarcode = null;
            Udc = null;
        }

        private async Task LoadData()
        {
            IsLoading = true;

            await Task.Factory.StartNew(() =>
             {
                 try
                 {
                     ParamManager.Init(Global.Instance.ConnGlobal);

                     LoadBays();

                     
                     if (Controller > 0)
                         Global.Instance.AddCallBackClientMfcAsync(Controller);
                 }
                 catch (Exception ex)
                 {
                     Global.ErrorAsync(_windowManager, ex.Message);
                 }
             });

            IsLoading = true;
        }

        /// <summary>
        /// Carico la lista delle baie gestite
        /// </summary>
        private void LoadBays()
        {
            if (BaysNr.Count > 0)
            {
                foreach (int item in BaysNr)
                {
                    HndBayBay bay = BaseBindableDbEntity.GetList<HndBayBay>(_conn, KeepConnectionOpen: true).FirstOrDefault(x => x.BAY_Num == item);
                    if (bay != null)
                    {
                        ManagedBays.Add(bay);
                    }
                }
            }
        }

        private async void LoadUdcFromDb(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                var udc = new AgilogDll.UdcUdc(_conn);
                if (udc.GetByKey(barcode, _conn, false))
                {
                    Udc = udc;

                    List<CompartementDTO> lstDetailDataUDC = await GetDetailDataUDC(UDCBarcode);
                    CompartmentDTO = lstDetailDataUDC;
                    //List<AgilogDll.UdcCompartment> compartments = new List<AgilogDll.UdcCompartment>();
                    //compartments = Udc.Compartments.ToList();
                    //List<AgilogDll.Item> items = BaseBindableDbEntity.GetList<AgilogDll.Item>((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), "WHERE ITM_Code <> [dbo].[mfn_Params_GetStr] ('ITM_Dummy', '_', '')").ToList();
                    //List<CompartementDTO> compartmentDT0 = new List<CompartementDTO>();

                    ////var risultato = lista1.Join(lista2,
                    ////elementoLista1 => elementoLista1.Campo1,
                    ////elementoLista2 => elementoLista2.Campo1,
                    ////(elementoLista1, elementoLista2) => new { Elemento1 = elementoLista1, Elemento2 = elementoLista2 })
                    ////.Where(joinResult => joinResult.Elemento1.Campo2 == joinResult.Elemento2.Campo2 && joinResult.Elemento1.Campo3 == joinResult.Elemento2.Campo3);

                    //var result = compartments.Join(items,
                    //compartment => compartment.UCM_ITM_Code,
                    //item => item.ITM_Code,
                    //(compartment, item) => new { item.ITM_Owner, item.ITM_Code, item.ITM_Desc, compartment.UCM_Owner, compartment.UCM_Stock, compartment.UCM_Batch, compartment.UCM_ExpiringDate })
                    //.Where(joinResult => joinResult.ITM_Owner == joinResult.UCM_Owner);

                    //foreach (var comp in result)
                    //{
                    //    CompartementDTO compartmentTmp = new CompartementDTO();
                    //    compartmentTmp.ItemOwner = comp.ITM_Owner;
                    //    compartmentTmp.ItemCode = comp.ITM_Code;
                    //    compartmentTmp.ItemDescription = comp.ITM_Desc;
                    //    compartmentTmp.ItemStock = comp.UCM_Stock;
                    //    compartmentTmp.ItemBatch = comp.UCM_Batch;
                    //    compartmentTmp.ItemExpirationDate = comp.UCM_ExpiringDate;
                    //    compartmentDT0.Add(compartmentTmp);
                    //}
                    //CompartmentDTO = compartmentDT0;
                }
                else
                {
                    Udc = null;

                    SnackBarMessage = Global.Instance.LangTl("Load unit not found !");
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotifyOfPropertyChange(() => HeightClass);
                    NotifyOfPropertyChange(() => LengthClass);
                    NotifyOfPropertyChange(() => WeightClass);
                    NotifyOfPropertyChange(() => WidthClass);
                    NotifyOfPropertyChange(() => UdcHasErrors);
                    //NotifyOfPropertyChange(() => IsToWarehouse);
                    //NotifyOfPropertyChange(() => IsTransit);
                    NotifyOfPropertyChange(() => CanConfirm);
                  
                });
            }
            else
            {
                Udc = null;
            }
        }

       
        public async Task<EOperationType> SelectOperationType()
        {
            var dialogSettings = new Dictionary<string, object>()
                {
                    { "BorderThickness", new Thickness(1) },
                    { "WindowState", WindowState.Normal },
                    { "ResizeMode", ResizeMode.NoResize },
                    { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                    { "SizeToContent", SizeToContent.WidthAndHeight },
                    { "Icon", Global.Instance.GetAppIcon() },
                    { "MinHeight", 240 },
                    { "MinWidth", 320 },
                };

            var elementList = new List<CustomComboBoxItem>
            {
                new CustomComboBoxItem(Global.Instance.LangTl("Insert in the warehouse").TrimUI(), EOperationType.ToWarehouse),
                new CustomComboBoxItem(Global.Instance.LangTl("Only wrap").TrimUI(), EOperationType.OnlyWrap)
            };

            var selection = new SelectElementViewModel(_windowManager, _eventAggregator, Global.Instance.LangTl("Choose the operation to perform"), elementList);
            if (! await  _windowManager.ShowDialogAsync(selection, settings: dialogSettings) == true ||
                selection.SelectedElement == null)
            {
                return await SelectOperationType();
            }

            return (EOperationType)selection.SelectedElement;
        }
        private async Task<List<CompartementDTO>> GetDetailDataUDC(string udc)
        {

            List<CompartementDTO> retval = null;
            string error = null;
            //await Task.Factory.StartNew(() =>
            //{
            try
            {
                DataTable dt = DbUtils.ExecuteDataTable($"msp_GetDetailDataUDC '{udc}' ", DbUtils.CloneConnection(Global.Instance.ConnGlobal), false);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //CompartementDTO compartementDTO = new CompartementDTO();
                    List<CompartementDTO> listCompartmentDTO = new List<CompartementDTO>();
                    foreach (DataRow row in dt.Rows)
                    {
                        CompartementDTO compartementDTO = new CompartementDTO();
                        compartementDTO.ItemCode = Convert.ToString(row.GetValue("ITM_CODE"));
                        compartementDTO.ItemOwner = Convert.ToInt32(row.GetValue("UCM_Owner"));
                        compartementDTO.ItemDescription = Convert.ToString(row.GetValue("ITM_DESC"));
                        compartementDTO.CompartmentNr = Convert.ToInt32(row.GetValue("UCM_Index"));
                        compartementDTO.ItemStock = Convert.ToDecimal(row.GetValue("LIN_Quantity"));
                        compartementDTO.ItemBatch = Convert.ToString(row.GetValue("UCM_Batch"));
                        compartementDTO.ItemExpirationDate = Convert.ToDateTime(row.GetValue("UCM_ExpiringDate"));
                        compartementDTO.CustomerOrderNr = Convert.ToString(row.GetValue("UDC_CustomerOrderNo"));
                        compartementDTO.OrderCode = Convert.ToString(row.GetValue("LIN_ORD_OrderCode"));
                        //compartementDTO.OrderLine = Convert.ToInt32(row.GetValue("LIN_Num"));
                        //compartementDTO.OrderCausal = Convert.ToString(row.GetValue("ORD_Causal"));

                        listCompartmentDTO.Add(compartementDTO);
                    }
                    retval = listCompartmentDTO;
                }
            }
            catch (Exception ex)
            {
                await Global.ErrorAsync(_windowManager, ex.Message);

            }

            //});
            return retval;

        }

        private void LoadMissionState()
        {
            Presenza = false;
            IsLoading = true;
            var mission = _utils.GetMissions((SqlConnection)Global.Instance.ConnGlobal, $"WHERE MIS_L1_MST_CODE = 'WSHP' AND MIS_Dest_BAY_NUM={BaysNr[0]} ").FirstOrDefault();
            // Se non trovo la missione oppure è una missione di evacuazione, esco
            if (mission == null)
            //if (mission == null || mission.EvacRequest != null)
            {
                IsLoading = false;
                Presenza = false;
                return;
            }

            //// Se ho incongruenza di tracking
            //if (barcode != mission.MIS_UDC_Code && udcType < 90) udcType = 98;
            Presenza = true;
            IsLoading = false;
            UDCBarcode = mission.MIS_UDC_Code;

        }

        private void LoadController()
        {

            var query = $@"SELECT * FROM MFC_CONTROLLERS WHERE CTR_Id = {Controller.SqlFormat()}";
            var dt = DbUtils.ExecuteDataTable(query, _conn, KeepOpen: false);

            _controller = dt.Rows[0].GetValue("CTR_Code");

        }
        #endregion Private methods

        #region Public Methods

        public async Task ConfirmAsync()
        {


            SendTelegramDEST();

            bool ret = true;
            //Direction = "E";
            //if (_CurrentBay != null && _CurrentBay.BAY_SCT_Code == "A" && !_ManualOperation) // se baia ingresso automatico e non è in funzionamento manuale
            //{

            //}
            //else if (_CurrentBay != null && _CurrentBay.BAY_BYT_Code == "IN" && _CurrentBay.BAY_SCT_Code == "M")
            //{
            //    switch (Direction)
            //    {
            //        case "E":
            //            ret = await Task.Run(() => { return _utils.Miss_NewExit_Custom(Udc.UDC_Code, _CurrentBay.BAY_Num); });
            //            break;
            //        case "T":
            //            ret = await Task.Run(() => { return _utils.Miss_NewOnlyWrap(Udc.UDC_Code, _CurrentBay.BAY_Num); });
            //            break;
            //        default:
            //            break;
            //    }

            //    //if(ret)
            //    //{
            //    //    DATA(Udc.UDC_Code);
            //    //}
            //    //else
            //    //{
            //    //    await Global.ErrorAsync(_windowManager, _utils.Error);
            //    //}

            //}
            //bool ret = true;
            //if (IsTransit)
            //{
            //    if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you want execute transit operation ?")))
            //    {
            //        return;
            //    }
            //    IsLoading = true;
            //    if (_CurrentBay != null && _CurrentBay.BAY_BYT_Code == "IN" && _CurrentBay.BAY_SCT_Code == "M")
            //    {
            //        ret = await Task.Run(() => { return _utils.Miss_NewOnlyWrap(Udc.UDC_Code, _CurrentBay.BAY_Num); });
            //    }
            //    IsLoading = false;

            //}
            //else
            //{
            //    if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you want execute warehouse Exit operation ?")))
            //    {
            //        return;
            //    }
            //    IsLoading = true;
            //    if (_CurrentBay != null && _CurrentBay.BAY_BYT_Code == "IN" && _CurrentBay.BAY_SCT_Code == "M")
            //    {
            //        ret = await Task.Run(() => { return _utils.Miss_NewEntrance_Custom(Udc.UDC_Code, _CurrentBay.BAY_Num); });
            //    }
            //    IsLoading = false;
            //}
            if (ret)
            {
                RemovePresence();
            }
            else
            {
                await Global.ErrorAsync(_windowManager, _utils.Error);
            }



        }

        private void SendTelegramDEST()
        {
            try
            {
                
               


                var mission = _utils.GetMissions((SqlConnection)Global.Instance.ConnGlobal, $"WHERE MIS_UDC_Code = {UDCBarcode.SqlFormat()}").FirstOrDefault();
                if (mission != null)
                {                    
                    Handling_Tel tel = new Handling_Tel(ETelegramTypes.DEST, _wcsIdentity, _controllerIdentity);
                    
                    tel.MissionID = mission.MIS_Id;
                    tel.TelegramType = ETelegramTypes.DEST;
                    //tel.Destination =  ;
                    
                    Global.Instance.SendTelegram(Controller, tel, true, 0, out string errorCode);                           
                }
                              
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }
        }

        public async Task ToWarehouseAsync()
        {
            /*
             * UDC di picking 
             * Essendo l’UDC già destinata alla vendita, deve sempre essere fasciata per la vendita ed etichettata
             */
            await ManageDirection();
        }

        private async Task ManageDirection()
        {
            IsToWarehouse = (IsToWarehouse) ? false : true;
            IsTransit = (IsTransit) ? false : true;
        }

        public async Task TransitAsync()
        {
            /*
             * UDC di picking 
             * Essendo l’UDC già destinata alla vendita, deve sempre essere fasciata per la vendita ed etichettata
             */

            await ManageDirection();

            //if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you confirm the selected operation ?")))
            //{
            //    return;
            //}

            //IsLoading = true;

           
            //bool ret = await Task.Run(() => { return _utils.Miss_NewOnlyWrap(Udc.UDC_Code, _CurrentBay.BAY_Num); });
           

            //IsLoading = false;

            //if (ret)
            //{
            //    RemovePresence();

            //    SnackBarMessage = Global.Instance.LangTl("Starting to insert the UDC...");
            //}
            //else
            //{
            //    await Global.ErrorAsync(_windowManager, _utils.Error);
            //}


        }

        #endregion Public Methods
    }
}