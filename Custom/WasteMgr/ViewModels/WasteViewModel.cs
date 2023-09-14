using AgilogDll;
using Caliburn.Micro;
using Caliburn.Micro.Validation;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WasteMgr.ViewModels
{
    [Export(typeof(WasteViewModel))]
    public class WasteViewModel : ValidatingScreen<WasteViewModel>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private SqlConnection _conn;

        private int _bayNr;
        private string _moduleCode;
        private int _controller;
        private bool _isEnabledReject = false;
        private bool _shapeControlVisible = false;
        private bool _barcodeError = false;
        private bool _barcodeLength = false;
        private bool _notExist = false;
        private bool _duplicated = false;
        private MisMission _mission;
        private mSwAgilogDll.UdcUdc _udc;
        private string _emptyPalletImage = "/Resources/PalletVuoto.png";
        private string _palletImage = "/Resources/PalletVuoto.png";
        private string _fullPalletImage = "/Resources/PalletPieno.png";

        #endregion

        #region Properties

        public string PalletImage
        {
            get => _palletImage;
            set
            {
                _palletImage = value;
                NotifyOfPropertyChange(() => PalletImage);
            }
        }

        public bool IsEnabledReject
        {
            get { return _isEnabledReject; }
            set
            {
                _isEnabledReject = value;
                NotifyOfPropertyChange(() => IsEnabledReject);
            }
        }

        public bool BarcodeError
        {
            get { return _barcodeError; }
            set
            {
                _barcodeError = value;
                NotifyOfPropertyChange(() => BarcodeError);
            }
        }

        public bool BarcodeLength
        {
            get { return _barcodeLength; }
            set
            {
                _barcodeLength = value;
                NotifyOfPropertyChange(() => BarcodeLength);
            }
        }

        public bool NotExist
        {
            get { return _notExist; }
            set
            {
                _notExist = value;
                NotifyOfPropertyChange(() => NotExist);
            }
        }

        public bool Duplicated
        {
            get { return _duplicated; }
            set
            {
                _duplicated = value;
                NotifyOfPropertyChange(() => Duplicated);
            }
        }

        public MisMission Mission
        {
            get { return _mission; }
            set
            {
                _mission = value;
                NotifyOfPropertyChange(() => Mission);
            }
        }

        public mSwAgilogDll.UdcUdc Udc
        {
            get { return _udc; }
            set
            {
                _udc = value;
                NotifyOfPropertyChange(() => Udc);
                NotifyOfPropertyChange(() => HeightClass);
                NotifyOfPropertyChange(() => LengthClass);
                NotifyOfPropertyChange(() => WeightClass);
                NotifyOfPropertyChange(() => WidthClass);
                NotifyOfPropertyChange(() => Weight_Real);
                NotifyOfPropertyChange(() => Item);
            }
        }


        public bool ShapeControlVisible
        {
            get { return _shapeControlVisible; }
            set
            {
                _shapeControlVisible = value;
                NotifyOfPropertyChange(() => ShapeControlVisible);
                NotifyOfPropertyChange(() => HeightClass);
                NotifyOfPropertyChange(() => LengthClass);
                NotifyOfPropertyChange(() => WeightClass);
                NotifyOfPropertyChange(() => WidthClass);
            }
        }

        public decimal? Weight_Real => Udc?.UDC_Weight_Real;

        public AgilogDll.Item Item => Udc != null ? BaseBindableDbEntity.GetList<AgilogDll.Item>(_conn, $"WHERE ITM_CODE = {Udc.Compartments.First().UCM_ITM_Code.SqlFormat()}").First() : null;
        public WhsCfgHeightClasse HeightClass => Udc?.HeightClass;
        public WhsCfgLengthClasse LengthClass => Udc?.LengthClass;
        public WhsCfgWeightClasse WeightClass => Udc?.WeightClass;
        public WhsCfgWidthClasse WidthClass => Udc?.WidthClass;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public WasteViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, int bayNr, bool reEntry = false)
        {
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            DisplayName = Global.Instance.LangTl("Waste");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _bayNr = bayNr;

            // Carico il controller associato al modulo
            string query = $@"SELECT TOP (1) MOD_CTR_Id 
                            FROM HND_MODULES 
                            JOIN HND_BAY_MODULES ON BMO_MOD_CODE = MOD_Code
                            JOIN HND_BAY_BAYS ON BMO_BAY_NUM = BAY_Num
                            WHERE BAY_Num = {bayNr.SqlFormat()}";

            _controller = (int)DbUtils.ExecuteScalar(query, Global.Instance.ConnGlobal);
            Global.Instance.OnTelegramManagement += Global_OnTelegramManagement;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnTelegramManagement -= Global_OnTelegramManagement;
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            ParamManager.Init(Global.Instance.ConnGlobal);
            LoadBayInfo();

            if (_controller > 0)
            {
                Global.Instance.AddCallBackClientMfcAsync(_controller);
            }
        }

        #endregion

        #region Global Events

        private void Global_OnTelegramManagement(object sender, GenericEventArgs e)
        {
            try
            {
                string message = e.Arguments[1].ToString();

                Handling_Tel telegram = new Handling_Tel();
                telegram.ParseReceivedMessage(message, out mSwDllMFC.Telegram response);

                if (telegram.TelegramType != ETelegramTypes.LCAP && telegram.TelegramType != ETelegramTypes.LPRE || telegram.Position != _moduleCode) return;

                // Se arriva un LCAP sul modulo corretto significa che è arrivato un pallet in scarto
                if (telegram.TelegramType == ETelegramTypes.LCAP)
                {
                    // Carico i dati della missione
                    Mission = BaseBindableDbEntity.GetList<MisMission>(_conn, $"WHERE MIS_Id = {telegram.MissionID.SqlFormat()}").FirstOrDefault();

                    if (Mission == null) return;

                    // Carico tutti i dati dell'udc e del relativo articolo 
                    //Udc = BaseBindableDbEntity.GetList<UdcUdc>(_conn, $"WHERE UDC_Code = {Mission.MIS_UDC_Code.SqlFormat()}").FirstOrDefault(); 
                    Udc = Mission.Udc;

                    if (Udc == null) return;

                    PalletImage = _fullPalletImage;
                    ShapeControlVisible = true;
                    IsEnabledReject = true;

                    BarcodeError = Udc.UDC_BookingCode == "NoBarcode";
                    BarcodeLength = Udc.UDC_BookingCode == "BarcodeLength";
                    NotExist = Udc.UDC_BookingCode == "NoOrder";
                    Duplicated = Udc.UDC_BookingCode == "Duplicated";

                    if (BarcodeError || BarcodeLength)
                    {
                        IsEnabledReject = false;
                        Udc.UDC_Code = null;
                    }

                    if (NotExist || Duplicated)
                    {
                        IsEnabledReject = false;
                    }
                }

                try
                {


                    // Pallet prelevato, pulisco tutti i dati
                    if (telegram.TelegramType == ETelegramTypes.LPRE)
                    {
                        //if (NotExist)
                        //{
                        //   var queryUdc = $@"DELETE FROM UDC_UDCS WHERE UDC_CODE = {Udc.UDC_Code.SqlFormat()}";
                        //    DbUtils.ExecuteNonQuery(query, _conn);
                        //}

                        // Creo un nuovo ExpUdc che verrà poi elaborato
                        //ExpManhattan expManhattan = new ExpManhattan(_conn)
                        //{
                        //    EXU_MsgCode = 57,
                        //    EXU_MsgType = "CONTAINERSTATUS",
                        //    EXU_ContainerId = Udc.UDC_Code,
                        //    EXU_ContainerState = "SCANNED",
                        //    EXU_LogicalDest = "I",
                        //    EXU_PhysicalDest = BaseBindableDbEntity.GetList<HndBayBay>(_conn, $"WHERE BAY_Num = {_bayNr.SqlFormat()}").FirstOrDefault().BAY_Desc,
                        //    EXU_ItemCode = Udc.Compartments.First().UCM_ITM_Code,
                        //    CreationTime = DateTime.Now,
                        //    ToUnite = false,
                        //    Filename = "EXP_UDC"
                        //};

                        //expManhattan.Add();

                        //string query = $@"DELETE FROM MIS_ORDERS_DETAILS WHERE ODT_UDC_Code = {Udc.UDC_Code.SqlFormat()} AND ODT_ORD_OrderCode = '_AVVISI INGRESSO_'";
                        //DbUtils.ExecuteNonQuery(query, _conn);

                        //query = $@"DELETE FROM UDC_UDCS WHERE UDC_CODE = {Udc.UDC_Code.SqlFormat()}";
                        //DbUtils.ExecuteNonQuery(query, _conn);

                        //ShapeControlVisible = false;
                        //IsEnabledReject = false;
                        //Udc = null;
                        //PalletImage = _emptyPalletImage;
                    }
                }
                catch (Exception ex)
                {
                    Global.Instance.Log(ex.Message, LogLevels.Fatal);
                    ShapeControlVisible = false;
                    IsEnabledReject = false;
                    Udc = null;
                    PalletImage = _emptyPalletImage;
                }
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Carico le informazioni sulla baia
        /// </summary>
        private void LoadBayInfo()
        {
            if (_bayNr > 0)
            {
                HndBayBay hndBayBay = BaseBindableDbEntity.GetList<HndBayBay>(_conn).FirstOrDefault(x => x.BAY_Num == _bayNr);

                if (hndBayBay != null)
                {
                    _moduleCode = hndBayBay.HndBayModules.OrderByDescending(x => x.BMO_Priority).FirstOrDefault().BMO_MOD_Code;
                    DisplayName = hndBayBay.BAY_Desc;
                }
            }
        }

        #endregion

        #region Public Methods

        //public async Task Reject()
        //{
        //    if ((bool)await Global.ConfirmOrCancelAsync(_windowManager, Global.Instance.LangTl("CONFERMI DI VOLER PORTARE L'UDC AL MAGAZZINO TRADIZIONALE?")))
        //    {
        //        // Creo un nuovo ExpUdc che verrà poi elaborato
        //        ExpManhattan expManhattan = new ExpManhattan(_conn)
        //        {
        //            EXU_MsgCode = 57,
        //            EXU_MsgType = "CONTAINERSTATUS",
        //            EXU_ContainerId = Udc.UDC_Code,
        //            EXU_ContainerState = "SCANNED",
        //            EXU_LogicalDest = "I",
        //            EXU_PhysicalDest = BaseBindableDbEntity.GetList<HndBayBay>(_conn, $"WHERE BAY_Num = {_bayNr.SqlFormat()}").FirstOrDefault().BAY_Desc,
        //            EXU_ItemCode = Udc.Compartments.First().UCM_ITM_Code,
        //            Filename = "EXP_UDC"
        //        };

        //        expManhattan.Add();

        //        string query = $@"DELETE FROM MIS_ORDERS_DETAILS WHERE ODT_UDC_Code = {Udc.UDC_Code.SqlFormat()}";
        //        DbUtils.ExecuteNonQuery(query, _conn);

        //        query = $@"DELETE FROM UDC_UDCS WHERE UDC_CODE = {Udc.UDC_Code.SqlFormat()}";
        //        DbUtils.ExecuteNonQuery(query, _conn);

        //        IsEnabledReject = false;
        //    }
        //}

        #endregion
    }
}
