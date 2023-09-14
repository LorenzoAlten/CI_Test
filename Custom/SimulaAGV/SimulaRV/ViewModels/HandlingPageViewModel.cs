using AgilogDll.EntitiesDepallettizer;
using Caliburn.Micro;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimulaRV.ViewModels
{
    [Export(typeof(HandlingPageViewModel))]
    class HandlingPageViewModel : Screen, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private ETelegramTypes _TelegramType;
        private int _PingMillisec;
        private string _Position;
        private long _MissionID;
        private string _UDC_Barcode;
        private int _UDC_Type;
        private int _Cla_Lenght;
        private int _Cla_Width;
        private int _Cla_Height;
        private int _Cla_Weight;
        private int _BoxesStoraged;
        private int _udcsMissing;
        private string _BayCode;
        private string _Destination;
        private ETrackingErrors _TrackingErrorCode;
        private EstspStatus _Status;
        private bool _CheckUDC;

        private List<SimulaHdl_Ctr> _controllers;
        private List<HndPath> _paths;
        private List<UdcCfgType> _udcTypes;
        private List<WhsCfgLengthClasse> _lenghts;
        private List<WhsCfgWidthClasse> _widths;
        private List<WhsCfgHeightClasse> _heights;
        private List<WhsCfgWeightClasse> _weights;

        #endregion

        #region Properties

        public ETelegramTypes TelegramType
        {
            get { return _TelegramType; }
            set
            {
                _TelegramType = value;
                NotifyOfPropertyChange(() => TelegramType);
            }
        }

        public int PingMillisec
        {
            get { return _PingMillisec; }
            set
            {
                _PingMillisec = value;
                NotifyOfPropertyChange(() => PingMillisec);
            }
        }

        public string Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                NotifyOfPropertyChange(() => Position);
            }
        }

        public long MissionID
        {
            get { return _MissionID; }
            set
            {
                _MissionID = value;
                NotifyOfPropertyChange(() => MissionID);
            }
        }

        public string UDCBarcode
        {
            get { return _UDC_Barcode; }
            set
            {
                _UDC_Barcode = value;
                NotifyOfPropertyChange(() => UDCBarcode);
            }
        }

        public int UDCType
        {
            get { return _UDC_Type; }
            set
            {
                _UDC_Type = value;
                NotifyOfPropertyChange(() => UDCType);
            }
        }

        public int ClaLenght
        {
            get { return _Cla_Lenght; }
            set
            {
                _Cla_Lenght = value;
                NotifyOfPropertyChange(() => ClaLenght);
            }
        }

        public int ClaWidth
        {
            get { return _Cla_Width; }
            set
            {
                _Cla_Width = value;
                NotifyOfPropertyChange(() => ClaWidth);
            }
        }

        public int ClaHeight
        {
            get { return _Cla_Height; }
            set
            {
                _Cla_Height = value;
                NotifyOfPropertyChange(() => ClaHeight);
            }
        }

        public int ClaWeight
        {
            get { return _Cla_Weight; }
            set
            {
                _Cla_Weight = value;
                NotifyOfPropertyChange(() => ClaWeight);
            }
        }

        public int BoxesStoraged
        {
            get { return _BoxesStoraged; }
            set
            {
                _BoxesStoraged = value;
                NotifyOfPropertyChange(() => BoxesStoraged);
            }
        }

        public int UdcsMissing
        {
            get { return _udcsMissing; }
            set
            {
                _udcsMissing = value;
                NotifyOfPropertyChange(() => UdcsMissing);
            }
        }

        public string BayCode
        {
            get { return _BayCode; }
            set
            {
                _BayCode = value;
                NotifyOfPropertyChange(() => BayCode);
            }
        }

        public string Destination
        {
            get { return _Destination; }
            set
            {
                _Destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
        }

        public ETrackingErrors TrackingErrorCode
        {
            get { return _TrackingErrorCode; }
            set
            {
                _TrackingErrorCode = value;
                NotifyOfPropertyChange(() => TrackingErrorCode);
            }
        }

        public EstspStatus Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public bool CheckData
        {
            get { return _CheckUDC; }
            set
            {
                _CheckUDC = value;
                NotifyOfPropertyChange(() => CheckData);
            }
        }

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

        public List<CustomComboBoxItem> Positions { get; set; }
        public List<CustomComboBoxItem> Destinations { get; set; }
        public List<CustomComboBoxItem> UDCTypes { get; set; }
        public List<CustomComboBoxItem> ClaLenghts { get; set; }
        public List<CustomComboBoxItem> ClaWidths { get; set; }
        public List<CustomComboBoxItem> ClaHeights { get; set; }
        public List<CustomComboBoxItem> ClaWeights { get; set; }
        public List<CustomComboBoxItem> TelegramTypes { get; set; }
        public List<CustomComboBoxItem> TrackingErrorCodes { get; set; }
        public List<CustomComboBoxItem> PossibleStatus { get; set; }

        public ObservableCollection<string> ReceivedTelegrams { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public HandlingPageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Handling");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;
        }

        #endregion

        #region ViewModel Override

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Init();
        }

        #endregion

        #region Public methods

        public async Task SendAsync()
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                var controller = AppViewModel.Instance.SelectedController;

                SimulaHdl_Tel telegram = new SimulaHdl_Tel(_TelegramType, controller.Code, "WCS");
                telegram.Position = Position;
                telegram.MissionID = MissionID;
                telegram.UDC_Barcode = UDCBarcode;
                telegram.UDC_Type = UDCType;
                telegram.Cla_Lenght = ClaLenght;
                telegram.Cla_Width = ClaWidth;
                telegram.Cla_Height = ClaHeight;
                telegram.Weight = ClaWeight;
                telegram.Destination = Destination;
                telegram.TrackingErrorCode = TrackingErrorCode;
                telegram.BoxesStoraged = BoxesStoraged;
                telegram.Status = Status;
                telegram.CheckData = CheckData;
                telegram.UdcsMissing = UdcsMissing;
                telegram.BayCode = BayCode;

                controller.SendTelegram(telegram.GetMessage(), telegram.GetSignature(), true);
            });

            IsLoading = false;
        }

        public void Clean()
        {
            TelegramType = ETelegramTypes.DREQ;
            Position = null;
            MissionID = 0;
            UDCBarcode = null;
            UDCType = 0;
            ClaWidth = 0;
            ClaLenght = 0;
            ClaHeight = 0;
            ClaWeight = 0;
            BoxesStoraged = 0;
            Destination = null;
            CheckData = false;
            UdcsMissing = 0;
            BayCode = null;
            TrackingErrorCode = ETrackingErrors.UNKNOWN;
            Status = EstspStatus.FOTO;
        }

        #endregion

        #region Private methods

        protected void Init()
        {
            IsLoading = true;

            Task.Run(() =>
            {
                _paths = HndPath.GetList<HndPath>(Global.Instance.ConnGlobal);
                _udcTypes = UdcCfgType.GetList<UdcCfgType>(Global.Instance.ConnGlobal);
                _lenghts = WhsCfgLengthClasse.GetList<WhsCfgLengthClasse>(Global.Instance.ConnGlobal);
                _widths = WhsCfgWidthClasse.GetList<WhsCfgWidthClasse>(Global.Instance.ConnGlobal);
                _heights = WhsCfgHeightClasse.GetList<WhsCfgHeightClasse>(Global.Instance.ConnGlobal);
                _weights = WhsCfgWeightClasse.GetList<WhsCfgWeightClasse>(Global.Instance.ConnGlobal);

                Positions = new List<CustomComboBoxItem>();
                foreach (string name in _paths.OrderBy(p => p.PAT_Step).Select(p => p.PAT_MOD_Code).Distinct())
                {
                    Positions.Add(new CustomComboBoxItem(name, name));
                }

                Destinations = new List<CustomComboBoxItem>();
                foreach (string name in _paths.OrderByDescending(p => p.PAT_Step).Select(p => p.PAT_MOD_Code).Distinct())
                {
                    Destinations.Add(new CustomComboBoxItem(name, name));
                }

                ClaLenghts = new List<CustomComboBoxItem>();
                _lenghts.ForEach(i => ClaLenghts.Add(new CustomComboBoxItem(i.LNG_Desc, i.LNG_Num)));

                ClaWidths = new List<CustomComboBoxItem>();
                _widths.ForEach(i => ClaWidths.Add(new CustomComboBoxItem(i.WDT_Desc, i.WDT_Num)));

                ClaHeights = new List<CustomComboBoxItem>();
                _heights.ForEach(i => ClaHeights.Add(new CustomComboBoxItem(i.HGT_Desc, i.HGT_Num)));

                TelegramTypes = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(ETelegramTypes)))
                {
                    TelegramTypes.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(ETelegramTypes), name)));
                }

                TrackingErrorCodes = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(ETrackingErrors)))
                {
                    TrackingErrorCodes.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(ETrackingErrors), name)));
                }

                PossibleStatus = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(EstspStatus)))
                {
                    PossibleStatus.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(EstspStatus), name)));
                }

                ReceivedTelegrams = new ObservableCollection<string>();

            }).ContinueWith(antecedent =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotifyOfPropertyChange(() => Positions);
                    NotifyOfPropertyChange(() => Destinations);
                    NotifyOfPropertyChange(() => UDCTypes);
                    NotifyOfPropertyChange(() => ClaLenghts);
                    NotifyOfPropertyChange(() => ClaWidths);
                    NotifyOfPropertyChange(() => ClaHeights);
                    NotifyOfPropertyChange(() => ClaWeights);
                    NotifyOfPropertyChange(() => TelegramTypes);
                    NotifyOfPropertyChange(() => TrackingErrorCodes);
                    NotifyOfPropertyChange(() => PossibleStatus);
                    NotifyOfPropertyChange(() => ReceivedTelegrams);

                    TelegramType = ETelegramTypes.DREQ;
                });

                IsLoading = false;
            });
        }

        #endregion

        #region Global Events

        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
        }

        public void Handle(bool message)
        {
            // Il valore di message indica se l'inizializzazione
            // di tutti i componenti è andata a buon fine
            if (!message) return;

            try
            {
                _controllers = new List<SimulaHdl_Ctr>(AppViewModel.Instance.Managers.Where(m => m.ControllerCollection != null && m.ControllerCollection.Count > 0).
                                                                                      SelectMany(m => m.ControllerCollection).
                                                                                      Where(c => c is SimulaHdl_Ctr).
                                                                                      Cast<SimulaHdl_Ctr>());
                _controllers.ForEach(c => c.OnTelegramReceived += OnTelegramReceived);
            }
            catch { }
        }

        private void OnTelegramReceived(object sender, GenericEventArgs e)
        {
            SimulaHdl_Tel telegram = new SimulaHdl_Tel();
            telegram.ParseReceivedMessage(e.Arguments[1].ToString(), out var response);

            if (telegram.TelegramType == ETelegramTypes.ACKT ||
                telegram.TelegramType == ETelegramTypes.NACK)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ReceivedTelegrams != null)
                {
                    ReceivedTelegrams.Insert(0, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} - {e.Arguments[1].ToString()}");
                    if (ReceivedTelegrams.Count > 100) ReceivedTelegrams.RemoveAt(100);
                }
            });

            if (telegram.TelegramType == ETelegramTypes.PING)
            {
                PingMillisec = telegram.PingMillisec;
                return;
            }

            if (telegram.TelegramType == ETelegramTypes.DATA)
            {
                TelegramType = ETelegramTypes.CTRL;
                Position = telegram.Position;
                MissionID = telegram.MissionID;
                UDCBarcode = telegram.UDC_Barcode;
                UDCType = telegram.UDC_Type;
                Destination = telegram.Destination;
            }
            else if (telegram.TelegramType == ETelegramTypes.DEST)
            {
                TelegramType = ETelegramTypes.LCAP;
                MissionID = telegram.MissionID;
                Destination = telegram.Destination;
            }
            else if (telegram.TelegramType == ETelegramTypes.LCDL)
            {
                TelegramType = ETelegramTypes.LPRE;
                MissionID = telegram.MissionID;
            }
            else if (telegram.TelegramType == ETelegramTypes.TKRD ||
                     telegram.TelegramType == ETelegramTypes.TKDL)
            {
                TelegramType = ETelegramTypes.TKDT;
                Position = telegram.Position;
            }
            else if (telegram.TelegramType == ETelegramTypes.TKUP)
            {
                TelegramType = ETelegramTypes.LCAP;
                Position = telegram.Position;
                MissionID = telegram.MissionID;
                UDCBarcode = telegram.UDC_Barcode;
                UDCType = telegram.UDC_Type;
                Destination = telegram.Destination;
            }
        }

        #endregion
    }
}
