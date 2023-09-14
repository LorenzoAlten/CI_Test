using AgilogDll.DTO;
using Caliburn.Micro;
using Microsoft.VisualBasic;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwAgilogDll.ViewModels;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EntranceMgr.ViewModels
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
    [Export(typeof(EntranceViewModel))]
    public class EntranceViewModel : Conductor<Screen>
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
        private List<CompartementDTO> _compartmentDTO;
        private object _lockObj = new object();

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
        private string _CauseMission = "E";
        public string _controller;
        /// <summary>
        /// Funzionamento manuale baia automatica
        /// Flag che consente di fermare le UDC in ingresso dalla baia automatica per consentire all’operatore la scelta fra stoccaggio e transito
        /// </summary>
        private bool _ManualOperation = false;
        private HndBayBay _CurrentBay;
        private bool _ExistingUDC = false;
        private DateTime _lastDateMissionFromEntrancePosition = DateTime.MinValue;
        private BackgroundWorker _SetFocus;

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
            }
        }

        public string UDCBarcode
        {
            get { return _UDCBarcode; }
            set
            {
                _UDCBarcode = value;
                NotifyOfPropertyChange(() => UDCBarcode);

                _ExistingUDC = CheckExistingUDCAsync(_UDCBarcode);

                if (!_ExistingUDC)
                    LoadUdcFromDb(_UDCBarcode);
                else
                {
                    UDCBarcode = "";
                    if (!_SetFocus.IsBusy)
                        _SetFocus.RunWorkerAsync();
                }
                //_UDCBarcode = "";
                //NotifyOfPropertyChange(() => UDCBarcode);
            }
        }
        public string CauseMission { get { return IsToWarehouse ? "E" : "T"; } }
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
                //return Presenza

                //        &&
                //        !string.IsNullOrWhiteSpace(UDCBarcode) &&
                //        Udc != null &&
                //        ((HeightClass != null && HeightClass.HGT_Waste) ||
                //        (LengthClass != null && LengthClass.LNG_Waste) ||
                //        (WeightClass != null && WeightClass.WGH_Waste) ||
                //        (WidthClass != null && WidthClass.WDT_Waste));

                return false;
            }
        }

        //public bool IsTransit { get { return CauseMission == "T" && Presenza; } }
        //public bool IsToWarehouse { get { return CauseMission == "E" && Presenza; } }
        private bool _IsTransit;

        public bool IsTransit
        {
            get { return _IsTransit; }
            set
            {
                _IsTransit = value;
                NotifyOfPropertyChange(() => IsTransit);
            }
        }
        private bool _IsToWarehouse;

        public bool IsToWarehouse
        {
            get { return _IsToWarehouse; }
            set
            {
                _IsToWarehouse = value;
                NotifyOfPropertyChange(() => IsToWarehouse);
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
                NotifyOfPropertyChange(() => WaitingUDC);
                NotifyOfPropertyChange(() => IsTransit);
                NotifyOfPropertyChange(() => IsToWarehouse);
                NotifyOfPropertyChange(() => CauseMission);
            }
        }

        public bool CanConfirm
        {
            get
            {
                return Presenza &&
                       (Udc != null) &&
                       (_CurrentBay != null)
                       //&&
                       //(_CurrentBay.BAY_SCT_Code == "M" || _ManualOperation)
                       ////&&
                       //Monoreference
                       ;
            }
        }
        #endregion Properties

        #region Constructor

        [ImportingConstructor]
        public EntranceViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, List<int> baysNr, int controller = 0, bool reEntry = false)
        {
            DisplayName = Global.Instance.LangTl("Entrance");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);

            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            BaysNr = baysNr;
            Controller = controller;

            _SetFocus = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = false
            };
            _SetFocus.DoWork += SetFocus_DoWork;


            //LoadController();
            Global.Instance.OnTelegramManagement += Global_OnTelegramManagement;
        }

        private void LoadController()
        {

            var query = $@"SELECT * FROM MFC_CONTROLLERS WHERE CTR_Id = {Controller.SqlFormat()}";
            var dt = DbUtils.ExecuteDataTable(query, _conn, KeepOpen: false);

            _controller = dt.Rows[0].GetValue("CTR_Code");

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


        Views.EntranceView View;

        protected override async void OnViewLoaded(object view)
        {
            View = (Views.EntranceView)view;

            IsLoading = true;

            await LoadData();

            IsLoading = false;

            if (!_SetFocus.IsBusy)
                _SetFocus.RunWorkerAsync();

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
            double ElapsedTime = DateTime.Now.Subtract(_lastDateMissionFromEntrancePosition).TotalSeconds;

            if (Presenza || ElapsedTime < 10) return;

            Presenza = true;

            _CurrentBay = ManagedBays.FirstOrDefault(bay => bay.HndBayModules.Any(mod => mod.BMO_MOD_Code == telegram.Position));

            if (_CurrentBay != null && _CurrentBay.BAY_BYT_Code == "IN" && _CurrentBay.BAY_SCT_Code == "A")
            {
                if (!string.IsNullOrEmpty(telegram.UDC_Barcode) &&
                    !telegram.UDC_Barcode_Error &&
                    telegram.UDC_Barcode_IsValid)
                {
                    UDCBarcode = telegram.UDC_Barcode;
                }
            }

            //if (!string.IsNullOrEmpty(telegram.UDC_Barcode) &&
            //    !telegram.UDC_Barcode_Error &&
            //    telegram.UDC_Barcode_IsValid)
            //{
            //    UDCBarcode = telegram.UDC_Barcode;
            //}
        }

        private void ProcessLCAP(TelegramBase telegram)
        {

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
            // Mi salvo l'ultima volta che ho "sbiancato" l'interfaccia
            _lastDateMissionFromEntrancePosition = DateTime.Now;
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

        private void SetFocus_DoWork(object sender, DoWorkEventArgs e)
        {
            IInputElement txtItemBarcode = null;

            while (string.IsNullOrEmpty(UDCBarcode))
            {
                OnUIThread(() =>
                {
                    try
                    {
                        if (txtItemBarcode == null) txtItemBarcode = (System.Windows.IInputElement)View.FindName("txtUDCBarcode");

                        IInputElement focusedControl = System.Windows.Input.Keyboard.FocusedElement;

                        //if (UdcBatch)
                        //{
                        //    System.Windows.Input.Keyboard.Focus(txtItemBarcode);
                        //    return;
                        //}

                        if (focusedControl != null)
                        {
                            System.Windows.Controls.Control control = focusedControl as System.Windows.Controls.Control;

                            if (control != null && !control.Name.Equals("txtUDCBarcode"))
                            {
                                System.Windows.Input.Keyboard.Focus(txtItemBarcode);
                            }
                        }
                    }
                    catch { }
                });

                Thread.Sleep(100);
            }
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
                //var udc = new AgilogDll.UdcUdc(_conn);
                var udc = new AgilogDll.UdcUdc(_conn);
                if (udc.GetByKey(barcode, _conn, false))
                {
                    Udc = udc;

                    List<CompartementDTO> lstDetailDataUDC = await GetDetailDataUDC(UDCBarcode);

                    CompartmentDTO = lstDetailDataUDC;

                    /*Come causale di default della missione  imposto il transito "T" se l'udc
                      ha associato un ordine oppure un ordine cliente, in caso contrario l'ingresso al magazzino*/
                    bool CheckOrderOnUDC = lstDetailDataUDC.Any(i => !(string.IsNullOrEmpty(i.OrderCode)) || !(string.IsNullOrEmpty(i.CustomerOrderNr)));

                    if (CheckOrderOnUDC)
                    {
                        IsTransit = true;
                        IsToWarehouse = false;
                    }
                    else
                    {
                        IsTransit = false;
                        IsToWarehouse = true;
                    }
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
                    NotifyOfPropertyChange(() => IsToWarehouse);
                    NotifyOfPropertyChange(() => IsTransit);

                });
            }
            else
            {
                Udc = null;
            }
        }

        private async Task<List<CompartementDTO>> GetDetailDataUDC(string udc)
        {

            List<CompartementDTO> retval = null;
            string error = null;
            //await Task.Factory.StartNew(() =>
            //{
            try
            {
                DataTable dt = DbUtils.ExecuteDataTable($"msp_GetDetailDataUDC  '{udc}'  ", DbUtils.CloneConnection(Global.Instance.ConnGlobal), false);
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

        private bool CheckExistingUDCAsync(string UdcBarcode)
        {
            string error = null;

            var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            var ret = _utils.CheckExistingUDC((SqlConnection)conn, UdcBarcode, Global.Instance.CurrentUser.Code, ref error);
            if (ret)
            {
                Global.AlertAsync(_windowManager, $"L'udc {UdcBarcode} inserito è già presente nel sistema");
                return true;
            }
            return false;
        }

        private DateTime CheckLastDateMissionFromEntrancePosition(string position)
        {
            string error = null;
            var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            return _utils.CheckLastDateMissionFromEntrancePosition((SqlConnection)conn, position, Global.Instance.CurrentUser.Code, ref error);

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
            if (!await _windowManager.ShowDialogAsync(selection, settings: dialogSettings) == true ||
                selection.SelectedElement == null)
            {
                return await SelectOperationType();
            }

            return (EOperationType)selection.SelectedElement;
        }

        #endregion Private methods

        #region Public Methods

        public async Task ConfirmAsync()
        {

            bool ret = true;

            switch (CauseMission)
            {
                case "T":
                    if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you want execute transit operation ?")))
                    {
                        return;
                    }
                    break;
                case "E":
                    if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you want execute warehouse entrance operation ?")))
                    {
                        return;
                    }
                    break;
                default:
                    break;
            }
            //Verifico che la baia sia quella di ingresso e sia quella manuale
            if (_CurrentBay != null && _CurrentBay.BAY_BYT_Code == "IN" && (_CurrentBay.BAY_SCT_Code == "M" || _CurrentBay.BAY_SCT_Code == "A"))
            {
                ret = await Task.Run(() => { return _utils.Miss_NewEntrance_Custom(Udc.UDC_Code, _CurrentBay.BAY_Num, CauseMission); });
            }
            if (ret)
            {
                RemovePresence();
            }
            else
            {
                await Global.ErrorAsync(_windowManager, _utils.Error);
            }
        }
        public async Task ToWarehouseAsync()
        {
            /*
             * UDC di picking 
             * Essendo l’UDC già destinata alla vendita, deve sempre essere fasciata per la vendita ed etichettata
             */
            await ManageDirection("E");
        }

        private async Task ManageDirection(string direction)
        {
            switch (direction)
            {
                case "E":
                    if (IsToWarehouse)
                    {
                        OnUIThread(() => Global.AlertAsync(_windowManager, Global.Instance.LangTl("L'UDC è gia stato impostato per andare a magazzino")));
                        return;
                    }
                    else
                    {
                        IsToWarehouse = true;
                        IsTransit = IsToWarehouse ? false : true;
                    }
                    break;
                case "T":
                    if (IsTransit)
                    {
                        OnUIThread(() => Global.AlertAsync(_windowManager, Global.Instance.LangTl("L'UDC è già stato impostato per andare in uscita")));
                        return;
                    }
                    else
                    {
                        IsTransit = true;
                        IsToWarehouse = IsTransit ? false : true;
                    }
                    break;
                default:
                    break;
            }
            NotifyOfPropertyChange(() => CauseMission);
            NotifyOfPropertyChange(() => IsTransit);
            NotifyOfPropertyChange(() => IsToWarehouse);
        }

        public async Task TransitAsync()
        {
            await ManageDirection("T");

        }

        #endregion Public Methods
    }
}