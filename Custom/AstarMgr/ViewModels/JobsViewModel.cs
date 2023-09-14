using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using mSwAgilogDll;
using mSwAgilogDll.MFC.Astar;
using mSwAgilogDll.MFC.Astar.Data;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AstarMgr.ViewModels
{
    [Export(typeof(JobsViewModel))]
    class JobsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private List<AstarJob> _jobsList = null;
        private List<AstarMessage> _messageList = null;
        private AstarJob _selectedJob = null;
        private List<LogEntry> _jobLogs = null;
        private ObservableCollection<AstarData> _messages = null;
        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private bool _logIsExpanded = false;
        private AstarMessage _selectedMessage;

        private bool _hideHealth = false;
        private bool _hideSent = false;

        private PackIcon _healthIcon;
        private PackIcon _sentIcon;

        #endregion

        #region Properties

        public List<AstarMessage> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                NotifyOfPropertyChange(() => MessageList);
            }
        }

        public AstarMessage SelectedMessage
        {
            get { return _selectedMessage; }
            set
            {
                _selectedMessage = value;
                NotifyOfPropertyChange(() => SelectedMessage);
            }
        }

        public string Code
        {
            get { return SelectedJob.ASJ_Code; }
            set { }
        }

        public List<AstarJob> JobsList
        {
            get { return _jobsList; }
            set
            {
                _jobsList = value;
                NotifyOfPropertyChange(() => JobsList);
            }
        }

        public AstarJob SelectedJob
        {
            get { return _selectedJob; }
            set
            {
                _selectedJob = value;
                RefreshLogs();

                NotifyOfPropertyChange(() => SelectedJob);
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
                NotifyOfPropertyChange(() => CanExecute);
            }
        }

        public ObservableCollection<AstarData> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                NotifyOfPropertyChange(() => Messages);
            }
        }

        public List<LogEntry> JobLogs
        {
            get { return _jobLogs; }
            set
            {
                _jobLogs = value;
                NotifyOfPropertyChange(() => JobLogs);
            }
        }

        public bool CanStart
        {
            get
            {
                return IsAdmin &&
                       (SelectedJob != null) &&
                       SelectedJob.State == EJobStates.Stopped &&
                       SelectedJob.ASJ_Enabled;
            }
        }

        public bool CanStop
        {
            get
            {
                return IsAdmin &&
                       (SelectedJob != null) &&
                       SelectedJob.State != EJobStates.Stopped;
            }
        }

        public bool CanEnable
        {
            get
            {
                return IsAdmin &&
                       (SelectedJob != null) &&
                       !SelectedJob.ASJ_Enabled;
            }
        }

        public bool CanDisable
        {
            get
            {
                return IsAdmin &&
                       (SelectedJob != null) &&
                       SelectedJob.State == EJobStates.Stopped &&
                       SelectedJob.ASJ_Enabled;
            }
        }

        public bool CanExecute
        {
            get
            {
                return IsAdmin &&
                       (SelectedJob != null) &&
                       SelectedJob.State == EJobStates.Started;
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

        public bool IsAdmin
        {
            get
            {
                return Global.Instance.CurrentUser != null &&
                       Global.Instance.CurrentUser.Sys == "True" &&
                       Global.Instance.CheckUserPerm();
            }
        }
        public ObservableCollection<TrafficController> ControllerCollection { get; set; } = new ObservableCollection<TrafficController>();

        public bool LogIsExpanded
        {
            get { return _logIsExpanded; }
            set
            {
                _logIsExpanded = value;
                NotifyOfPropertyChange(() => LogIsExpanded);

                if (_logIsExpanded) RefreshLogs();
            }
        }

        public ICommand RowCopyCommand { get; }

        public PackIcon HealthIcon
        {
            get
            {
                if (_healthIcon == null)
                    _healthIcon = new PackIcon();
                _healthIcon.Kind = _hideHealth ? PackIconKind.LeakOff : PackIconKind.Leak;

                return _healthIcon;
            }
        }

        public PackIcon SentIcon
        {
            get
            {
                if (_sentIcon == null)
                    _sentIcon = new PackIcon();
                _sentIcon.Kind = _hideSent ? PackIconKind.ShareOff : PackIconKind.Share;

                return _sentIcon;
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public JobsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            //DisplayName = Global.Instance.LangTl("Task list");
            DisplayName = "Astar Manager";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);

            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;
            Global.Instance.OnCurUSRChanged += Instance_OnCurUSRChanged;

            RowCopyCommand = new CustomCommandImplementation(MessageCopy);
        }

        #endregion

        #region ViewModel Override

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            RefreshJobs();
            RefreshMessages();
            NotifyOfPropertyChange(() => HealthIcon);
            NotifyOfPropertyChange(() => SentIcon);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!Global.Instance.AppClosing)
                Global.Instance.RefreshCurrentUser();

            await base.OnActivateAsync(cancellationToken);
        }

        #endregion

        #region Public methods

        public async Task StartExotecTestAsync()
        {
            var vm = new ExotecTestViewModel(_windowManager, _eventAggregator);
            var dialogSettings = new Dictionary<string, object>()
                {
                    { "BorderThickness", new Thickness(1) },
                    { "WindowState", WindowState.Normal },
                    { "ResizeMode", ResizeMode.NoResize },
                    { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                    { "SizeToContent", SizeToContent.WidthAndHeight },
                    { "Icon", Global.Instance.GetAppIcon() },
                    { "Title", "" },
                    { "MinHeight", 900 },
                    { "MinWidth", 1000 },
                    { "MaxHeight", 900 },
                    { "MaxWidth", 1000 },
                };
            var retVal = await _windowManager.ShowDialogAsync(vm, settings: dialogSettings);

            if (retVal == true) { RefreshMessages(); }

        }

        public void RefreshJobs()
        {
            Task.Run(() =>
            {
                IsLoading = true;

                JobsList = Shared.Managers.SelectMany(m => ((Astar_Mgr)m).Jobs).ToList();

                SelectedJob = null;
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });
        }

        public void Start()
        {
            if (SelectedJob.Start())
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
                NotifyOfPropertyChange(() => CanExecute);
            }
        }

        public void Stop()
        {
            if (SelectedJob.Stop())
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
                NotifyOfPropertyChange(() => CanExecute);
            }
        }

        public void Enable()
        {
            if (SelectedJob.SetEnabling(true, Code))
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
            }
        }

        public void Disable()
        {
            if (SelectedJob.SetEnabling(false, Code))
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
                NotifyOfPropertyChange(() => CanExecute);
            }
        }

        public void Execute()
        {
            if (SelectedJob.ForceJobExecution())
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
                NotifyOfPropertyChange(() => CanExecute);
            }
        }

        public void RefreshMessages()
        {
            Task.Run(() =>
            {
                IsLoading = true;

                string condition = "WHERE 1=1";
                if (_hideHealth)
                    condition += " AND ASM_Class NOT LIKE '%Health%'";
                if (_hideSent)
                    condition += " AND ASM_Sent = 0";

                MessageList = AstarMessage.Message((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), condition: condition);

                SelectedMessage = null;
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });
        }

        public void HideHealth()
        {
            _hideHealth = !_hideHealth;
            NotifyOfPropertyChange(() => HealthIcon);

            RefreshMessages();
        }

        public void HideSent()
        {
            _hideSent = !_hideSent;
            NotifyOfPropertyChange(() => SentIcon);

            RefreshMessages();
        }

        public void MessageCopy(object data)
        {
            if (SelectedMessage == null) return;

            try
            {
                Clipboard.SetText(SelectedMessage.ASM_Message);
                SnackBarMessage = Global.Instance.LangTl("Message copied successfully to clipboard");
            }
            catch { }
        }

        #endregion

        #region Private methods

        private void RefreshLogs()
        {
            Task.Run(() =>
            {
                if (_selectedJob != null)
                {
                    IsLoading = true;

                    JobLogs = Logger.GetLogs(SelectedJob.ASJ_Code, 100);
                }
                else
                {
                    JobLogs = Logger.GetLogs(null, 100);
                }
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });
        }

        #endregion

        #region Global Events

        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            //// Ogni 5 secondi rinfresco i messaggi
            //if ((Global.Instance.Repetitions1Sec % 5) != 0) return;

            //RefreshMessages();

            // Ogni 30 secondi rinfresco i log
            if ((Global.Instance.Repetitions1Sec % 10) != 0 || !LogIsExpanded) return;

            RefreshLogs();
        }

        private void Instance_OnCurUSRChanged(object sender, GenericEventArgs e)
        {
            NotifyOfPropertyChange(() => CanStart);
            NotifyOfPropertyChange(() => CanStop);
            NotifyOfPropertyChange(() => CanEnable);
            NotifyOfPropertyChange(() => CanDisable);
            NotifyOfPropertyChange(() => CanExecute);
        }

        #endregion
    }
}
