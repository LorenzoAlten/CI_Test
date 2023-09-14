using SimulaRV;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using mSwDllMFC;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;

namespace SimulaRV.ViewModels
{
    [Export(typeof(ShuttlePageViewModel))]
    class ShuttlePageViewModel : Screen, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private ETelegramTypes _TelegramType;
        private int _PingMillisec;
        private long _MissionID;
        private int _Cradle;
        private EMachineCommand_Results _CommandResult;
        private EMachineLocationTypes _LocationType;
        private int _RackNum;
        private int _UdcCount;
        private string _UDCBarcode;
        private int _X;
        private int _Y;
        private int _Z;
        private int _W;
        private int _QuotaX;
        private int _QuotaY;
        private int _QuotaZ;
        private int _QuotaW;

        private string _SelVariable;
        private ESimulaShuttleCtr_Variables _VariableShuttle;
        private ERVTrasloCtr_Variables _VariableTraslo;

        private int _VariableValue;

        private List<SimulaShuttle_Ctr> _controllers;

        private bool? _isTraslo;
        private bool? _isShuttle;

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

        public long MissionID
        {
            get { return _MissionID; }
            set
            {
                _MissionID = value;
                NotifyOfPropertyChange(() => MissionID);
            }
        }

        public int Cradle
        {
            get { return _Cradle; }
            set
            {
                _Cradle = value;
                NotifyOfPropertyChange(() => Cradle);
            }
        }

        public EMachineCommand_Results CommandResult
        {
            get { return _CommandResult; }
            set
            {
                _CommandResult = value;
                NotifyOfPropertyChange(() => CommandResult);
            }
        }

        public EMachineLocationTypes LocationType
        {
            get { return _LocationType; }
            set
            {
                _LocationType = value;
                NotifyOfPropertyChange(() => LocationType);
            }
        }

        public int RackNum
        {
            get { return _RackNum; }
            set
            {
                _RackNum = value;
                NotifyOfPropertyChange(() => RackNum);
            }
        }

        public int UdcCount
        {
            get { return _UdcCount; }
            set
            {
                _UdcCount = value;
                NotifyOfPropertyChange(() => UdcCount);
            }
        }

        public string UDCBarcode
        {
            get { return _UDCBarcode; }
            set
            {
                _UDCBarcode = value;
                NotifyOfPropertyChange(() => UDCBarcode);
            }
        }

        public int X
        {
            get { return _X; }
            set
            {
                _X = value;
                NotifyOfPropertyChange(() => X);
            }
        }

        public int Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                NotifyOfPropertyChange(() => Y);
            }
        }

        public int Z
        {
            get { return _Z; }
            set
            {
                _Z = value;
                NotifyOfPropertyChange(() => Z);
            }
        }

        public int W
        {
            get { return _W; }
            set
            {
                _W = value;
                NotifyOfPropertyChange(() => W);
            }
        }

        public int QuotaX
        {
            get { return _QuotaX; }
            set
            {
                _QuotaX = value;
                NotifyOfPropertyChange(() => QuotaX);
            }
        }

        public int QuotaY
        {
            get { return _QuotaY; }
            set
            {
                _QuotaY = value;
                NotifyOfPropertyChange(() => QuotaY);
            }
        }

        public int QuotaZ
        {
            get { return _QuotaZ; }
            set
            {
                _QuotaZ = value;
                NotifyOfPropertyChange(() => QuotaZ);
            }
        }

        public int QuotaW
        {
            get { return _QuotaW; }
            set
            {
                _QuotaW = value;
                NotifyOfPropertyChange(() => QuotaW);
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

        public string SelVariable
        {
            get { return _SelVariable; }
            set
            {
                _SelVariable = value;
                if (IsTraslo == true)
                {
                    //VariableTraslo = Enum.GetName(typeof(ERVTrasloCtr_Variables), VariableTraslo);
                    VariableTraslo = (ERVTrasloCtr_Variables)Enum.Parse(typeof(ERVTrasloCtr_Variables), (_SelVariable == null) ? ERVTrasloCtr_Variables.QuotaY.ToString() : _SelVariable, true);
                }
                if (IsShuttle == true)
                {
                    VariableShuttle = (ESimulaShuttleCtr_Variables)Enum.Parse(typeof(ESimulaShuttleCtr_Variables), (_SelVariable == null) ? ESimulaShuttleCtr_Variables.QuotaY.ToString() : _SelVariable, true);
                }
                NotifyOfPropertyChange(() => SelVariable);

            }
        }
        public ESimulaShuttleCtr_Variables VariableShuttle
        {
            get { return _VariableShuttle; }
            set
            {
                _VariableShuttle = value;
                NotifyOfPropertyChange(() => VariableShuttle);
            }
        }

        public ERVTrasloCtr_Variables VariableTraslo
        {
            get { return _VariableTraslo; }
            set
            {
                _VariableTraslo = value;
                NotifyOfPropertyChange(() => VariableTraslo);
            }
        }

        public int VariableValue
        {
            get { return _VariableValue; }
            set
            {
                _VariableValue = value;
                NotifyOfPropertyChange(() => VariableValue);
            }
        }

        public bool? IsTraslo
        {
            get { return _isTraslo; }
            set
            {
                _isTraslo = value;
                NotifyOfPropertyChange(() => IsTraslo);
                if (_isTraslo.Value)
                {
                        PublicVariables.Clear();
                        foreach (string name in Enum.GetNames(typeof(ERVTrasloCtr_Variables)))
                        { PublicVariables.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(ERVTrasloCtr_Variables), name))); }
                        NotifyOfPropertyChange(() => PublicVariables);
                        VariableTraslo = ERVTrasloCtr_Variables.QuotaY;
                        SelVariable = Enum.GetName(typeof(ERVTrasloCtr_Variables), VariableTraslo);
                }
            }
        }

        public bool? IsShuttle
        {
            get { return _isShuttle; }
            set
            {
                _isShuttle = value;
                NotifyOfPropertyChange(() => IsShuttle);
                if (_isShuttle.Value)
                {
                        PublicVariables.Clear();
                        foreach (string name in Enum.GetNames(typeof(ESimulaShuttleCtr_Variables)))
                        { PublicVariables.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(ESimulaShuttleCtr_Variables), name))); }
                        NotifyOfPropertyChange(() => PublicVariables);
                        VariableShuttle = ESimulaShuttleCtr_Variables.QuotaY;
                        SelVariable = Enum.GetName(typeof(ESimulaShuttleCtr_Variables), VariableShuttle);
                }
                
            }
        }

        public List<CustomComboBoxItem> TelegramTypes { get; set; }
        public List<CustomComboBoxItem> CommandResults { get; set; }
        public List<CustomComboBoxItem> LocationTypes { get; set; }
        public List<CustomComboBoxItem> PublicVariables_old { get; set; }
        public ObservableCollection<CustomComboBoxItem> PublicVariables { get; set; }

        public ObservableCollection<string> ReceivedTelegrams { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ShuttlePageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Shuttle");

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

                SimulaShuttle_Tel telegram = new SimulaShuttle_Tel(_TelegramType, controller.Code, "WCS");
                telegram.MissionID = MissionID;
                telegram.MachineID = controller.Code;

                var cradle = new SimulaShuttle_Tel.ShuttleCradleCommand();
                cradle.CradleID = 1;
                cradle.CommandResult = CommandResult;
                cradle.LocationType = LocationType;
                cradle.RackNum = RackNum;
                cradle.UdcCount = UdcCount;
                cradle.CradleCapacity = UdcCount;
                cradle.X = X;
                cradle.Y = Y;
                cradle.Z = Z;
                cradle.W = W;
                cradle.QuotaX = QuotaX;
                cradle.QuotaY = QuotaY;
                cradle.QuotaZ = QuotaZ;
                cradle.QuotaW = QuotaW;
                telegram.CradleCommands.Add(cradle);

                cradle.UdcDatas.Add(new SimulaShuttle_Tel.ShuttleUdcData
                {
                    MissionID = telegram.MissionID,
                    UdcBarcode = UDCBarcode,
                    UdcType = 0,
                });

                controller.SendTelegram(telegram.GetMessage(), telegram.GetSignature(), true);
            });

            IsLoading = false;
        }

        public void Clean()
        {

        }
        public async Task WriteVariableAsync()
        {
            if (!(AppViewModel.Instance.SelectedController is SimulaShuttle_Ctr))
            {
                Global.Error(_windowManager, "Controller selezionato non valido");
                return;
            }

            var retVal = 0;

            if (IsShuttle.Value)
            {
                var retValAsync = await ((SimulaShuttle_Ctr)AppViewModel.Instance.SelectedController).WriteVariableShuttleAsync(VariableShuttle, VariableValue);
                if (retValAsync != "OKWD") { retVal = 1; }
            }
            else if (IsTraslo.Value)
            {
                string stccode = AppViewModel.Instance.SelectedController.Code.ToString();

                var retValAsync = await ((SimulaShuttle_Ctr)AppViewModel.Instance.SelectedController).WriteVariableTrasloAsync(VariableTraslo, VariableValue,stccode);
                if (retValAsync != "OKWD") { retVal = 1; }
            }

            if (retVal == 1)
            {
                Global.Error(_windowManager, "Errore di scrittura variabile. Verifica la connessione e riprova");
                return;
            }

            SnackBarMessage = "Variabile scritta con successo";
        }

        #endregion

        #region Protected methods

        protected void Init()
        {
            IsLoading = true;

            Task.Run(() =>
            {                
                TelegramTypes = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(ETelegramTypes)))
                {
                    TelegramTypes.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(ETelegramTypes), name)));
                }
                CommandResults = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(EMachineCommand_Results)))
                {
                    CommandResults.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(EMachineCommand_Results), name)));
                }
                LocationTypes = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(EMachineLocationTypes)))
                {
                    LocationTypes.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(EMachineLocationTypes), name)));
                }
                PublicVariables = new ObservableCollection<CustomComboBoxItem>();
                ReceivedTelegrams = new ObservableCollection<string>();

            }).ContinueWith(antecedent =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UdcCount = 1;

                    NotifyOfPropertyChange(() => TelegramTypes);
                    NotifyOfPropertyChange(() => CommandResults);
                    NotifyOfPropertyChange(() => LocationTypes);
                    NotifyOfPropertyChange(() => PublicVariables);

                    TelegramType = ETelegramTypes.DONE;
                    CommandResult = EMachineCommand_Results.NOOP;
                    LocationType = EMachineLocationTypes.MG;
                    IsShuttle = true;
                    IsTraslo = false;
                    if (IsShuttle.Value)
                    {
                        //test 1 shuttle
                        SelVariable = Enum.GetName(typeof(ESimulaShuttleCtr_Variables), VariableShuttle);
                    }
                    if (IsTraslo.Value)
                    {
                        //test 1 traslo
                        SelVariable = Enum.GetName(typeof(ERVTrasloCtr_Variables), VariableTraslo);
                    }
                    /*
                    Variable = ESimulaShuttleCtr_Variables.QuotaY;
                    */
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
                _controllers = new List<SimulaShuttle_Ctr>(AppViewModel.Instance.Managers.Where(m => m.ControllerCollection != null && m.ControllerCollection.Count > 0).
                                                                                          SelectMany(m => m.ControllerCollection).
                                                                                          Where(c => c is SimulaShuttle_Ctr).
                                                                                          Cast<SimulaShuttle_Ctr>());
                _controllers.ForEach(c => c.OnTelegramReceived += OnTelegramReceived);
            }
            catch { }
        }

        private void OnTelegramReceived(object sender, GenericEventArgs e)
        {
            SimulaShuttle_Tel telegram = new SimulaShuttle_Tel();
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

            if (telegram.TelegramType == ETelegramTypes.MOVE)
            {
                TelegramType = ETelegramTypes.DONE;
                MissionID = telegram.MissionID;

                var cradle = telegram.CradleCommands.First();

                switch (cradle.Command)
                {
                    case EMachineCommands.CHECK:
                        CommandResult = EMachineCommand_Results.CHECK_OK;
                        break;

                    case EMachineCommands.DRIVE:
                        CommandResult = EMachineCommand_Results.DRIVE_OK;
                        break;

                    case EMachineCommands.LOAD:
                        CommandResult = EMachineCommand_Results.LOAD_OK;
                        break;

                    case EMachineCommands.UNLOAD:
                        CommandResult = EMachineCommand_Results.UNLOAD_OK;
                        break;
                }

                LocationType = cradle.LocationType;
                RackNum = cradle.RackNum;
                UdcCount = cradle.UdcCount;
                X = cradle.X;
                Y = cradle.Y;
                Z = cradle.Z;
                W = cradle.W;
                QuotaX = cradle.QuotaX;
                QuotaY = cradle.QuotaY;
                QuotaZ = cradle.QuotaZ;
                QuotaW = cradle.QuotaW;

                if (cradle.Command == EMachineCommands.DRIVE && QuotaY > 0)
                {
                    VariableShuttle = ESimulaShuttleCtr_Variables.QuotaY;
                    VariableValue = QuotaY;
                }

                if (cradle.UdcDatas.Count > 0)
                {
                    UDCBarcode = cradle.UdcDatas[0].UdcBarcode;
                }
            }
        }

        #endregion
    }
}
