using Movitrans;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using mSwDllMFC;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Windows.Data;
using System.Globalization;
using Movitrans.ViewModels;
using System.Dynamic;
using System.Threading;

namespace Movitrans.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;
        private static AppViewModel _instance;

        private SEWMovitrans _selectedMovitrans;

        private string _manager;

        #endregion

        #region Properties

        public ObservableCollection<SEWMovitrans> SewMovitrans { get; set; } = new ObservableCollection<SEWMovitrans>();
        public SEWMovitrans SelectedMovitrans
        {
            get
            {
                return _selectedMovitrans;
            }
            set
            {
                _selectedMovitrans = value;
                NotifyOfPropertyChange(()=>SelectedMovitrans);
            }
        }
        public bool IsLoading
        {
            get { return _LoadingIsVisible; }
            private set
            {
                _LoadingIsVisible = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }
        public DateTime Now
        {
            get { return _Now; }
            private set
            {
                _Now = value;
                NotifyOfPropertyChange(() => Now);
            }
        }
        public List<TrafficManager> Managers { get; set; }
        public ObservableCollection<TrafficManager> ManagerCollection { get; set; }
        public static AppViewModel Instance { get { return _instance; } }
        public IWindowManager WindowManager { get { return _windowManager; } }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = "Movitrans Manager";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            _instance = this;
            _manager = Global.Instance.CmdLineArgs[1];

            LoadManagers();
        }

        #endregion

        #region ViewModel Override

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;

                Managers.ForEach(m => m.Dispose());
            }

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public override async Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            if (!await Global.ConfirmAsync(_windowManager, "Sei sicuro di voler uscire? Tutte gli AGV si spegneranno")) return false;

            return base.CanCloseAsync(cancellationToken).Result;
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Aggiorno la data/ora
            Now = DateTime.Now;
        }

        #endregion

        #region Private Methods

        private async Task LoadManagers()
        {
            IsLoading = true;
            Managers = null;

            await Task.Run(() =>
            {
                var query = $@"
                    SELECT * FROM [MFC_MANAGERS]
                    WHERE MMG_Code LIKE {_manager.SqlFormat()}";
                var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);
                if (dt == null || dt.Rows.Count <= 0) return;

                Managers = new List<TrafficManager>();
                foreach (DataRow row in dt.Rows)
                {
                    Type type = Utils.GetTypeFromClassName(row.GetValue("MMG_Class"));
                    var manager = Activator.CreateInstance(type, Global.Instance.ConnGlobal, row) as TrafficManager;
                    Managers.Add(manager);
                }
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });

            if (Managers == null)
            {
                // Notifico ai ViewModel l'inizializzazione fallita
                await _eventAggregator.PublishOnUIThreadAsync(false);

                Utils.Error(Global.Instance.LangTl("Cannot initialize traffic manager. Check application Logs"));
                return;
            }

            ManagerCollection = new ObservableCollection<TrafficManager>();
            NotifyOfPropertyChange(() => ManagerCollection);

            Managers.ForEach(m => ManagerCollection.Add(m));

            foreach(Movitrans_Mgr mgr in Managers.Where(m=>m is Movitrans_Mgr))
            {
                foreach(SEWMovitrans s_mov in mgr.SewMovitrans)
                {
                    SewMovitrans.Add(s_mov);
                }
            }
            SelectedMovitrans = SewMovitrans.FirstOrDefault();

            // Notifico ai ViewModel l'inizializzazione avvenuta
            await _eventAggregator.PublishOnUIThreadAsync(true);
        }

        #endregion

        #region Public Methods

        public void MOVReset()
        {
            SelectedMovitrans.ResetRequest = true;
        }

        public void EditConfiguration()
        {
            dynamic settings = new ExpandoObject();
            settings.Height = 200;
            settings.Width = 350;
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Normal;
            settings.SizeToContent = SizeToContent.WidthAndHeight;

            _windowManager.ShowDialogAsync(new EnSavingCfgViewModel(_windowManager, _eventAggregator, (Movitrans_Mgr)Managers.FirstOrDefault(m => m is Movitrans_Mgr)), null, settings);
        }

        #endregion
    }
}

namespace Movitrans
{
    public class ScbState : BaseBindableObject
    {
        protected EChannelStates _networkState;

        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public EChannelStates NetworkState
        {
            get { return _networkState; }
            set { SetProperty(ref _networkState, value); }
        }

        public ScbState(int id, string name) : base()
        {
            Id = id;
            Name = name;
            NetworkState = EChannelStates.Unknown;
        }
    }
    public class SEWMovitrans : BaseBindableObject
    {
        private bool _enabled;
        private short _setPoint;
        private MOV_Status_Word _status;
        private double _outputPower;
        private short _temperature;
        private short _utilization;
        private short _warningCode;
        private short _errorCode;
        private string _code;
        public bool ResetRequest;
        public int _controllerId;

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }
        public short ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
                OnPropertyChanged("ErrorCode");
            }
        }
        public short WarningCode
        {
            get
            {
                return _warningCode;
            }
            set
            {
                _warningCode = value;
                OnPropertyChanged("WarningCode");
            }
        }
        public short Utilization
        {
            get
            {
                return _utilization;
            }
            set
            {
                _utilization = value;
                OnPropertyChanged("Utilization");
            }
        }
        public short Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }
        public double OutputPower
        {
            get
            {
                return _outputPower;
            }
            set
            {
                _outputPower = value;
                OnPropertyChanged("OutputPower");
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (!value)
                {
                    if (!Global.ConfirmAsync(AppViewModel.Instance.WindowManager, "Sei sicuro di voler disabilitare questo Movitrans? Tutte gli AGV su di esso si spegneranno").Result) return;
                }

                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
        public MOV_Status_Word Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        public short SetPoint
        {
            get
            {
                return _setPoint;
            }
            set
            {
                _setPoint = value;
                OnPropertyChanged("SetPoint");
            }
        }

        public int ControllerId
        {
            get
            {
                return _controllerId;
            }
            set
            {
                _controllerId = value;
            }
        }
        public SEWMovitrans(string code)
        {
            Code = code;
        }
    }
    public class MultiFlagToBooleanConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value[0] is MOV_Status_Word))
                return false;
            MOV_Status_Word State = (MOV_Status_Word)value[0];

            if (!(value[1] is MOV_Status_Word))
                return false;
            MOV_Status_Word Flag = (MOV_Status_Word)value[1];

            return State.HasFlag(Flag);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
