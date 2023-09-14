using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using mSwDllGrpc;

namespace TrafficMgr.ViewModels
{
    [Export(typeof(MainPageViewModel))]
    class MainPageViewModel : Screen, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private string _notifications;

        #endregion

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

        public string Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications += value + Environment.NewLine;
                NotifyOfPropertyChange(() => Notifications);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MainPageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Traffic Manager");

            _notifications = string.Empty;

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
        }

        #endregion

        #region ViewModel Override

        #endregion

        #region Public methods

        public async Task ViewTelegramsAsync()
        {
            var vm = new TelegramsViewModel(_windowManager, _eventAggregator);
            await _windowManager.ShowWindowAsync(vm);
        }

        public async Task ViewLogsAsync()
        {
            var vm = new LogsViewModel(_windowManager, _eventAggregator);
            await _windowManager.ShowWindowAsync(vm);
        }

        #endregion

        #region Private methods

        private bool InitGrpc()
        {
            // Istanzio il ServiceMgr
            var serviceMgr = new TelegramServiceMgr(TrafficUtils.Managers.Distinct().ToList());

            serviceMgr.OnNotify += (s, e) =>
            {
                var message = e.Argument.ToString();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Notifications = message;
                });
            };

            serviceMgr.OnTelegram += (s, e) =>
            {
                var received = (bool)e.Arguments[0];
                var controller = e.Arguments[1].ToString();
                var message = e.Arguments[2].ToString();

                var entry = new MsgEntry
                {
                    Timestamp = DateTime.Now,
                    Sender = received ? controller : "TrafficMgr",
                    Dest = received ? "TrafficMgr" : controller,
                    Message = message
                };

                try
                {
                    lock (Manager.Instance.MsgEntries)
                    {
                        if (Manager.Instance.MsgEntries.Count > 10000)
                            Manager.Instance.MsgEntries.RemoveAt(0);
                        Manager.Instance.MsgEntries.Add(entry);
                    }
                }
                catch { }
            };

            // Istanzio i controller e apro il canale Grpc
            if (serviceMgr.Initialize())
            {
                serviceMgr.OpenTelegramServer();
            }
            else return false;

            return true;
        }

        #endregion

        #region Events

        public async Task HandleAsync(bool message, CancellationToken cancellationToken)
        {
            // Il valore di message indica se l'inizializzazione
            // di tutti i componenti è andata a buon fine
            if (!message) return;

            if (!InitGrpc())
            {
                Utils.Error("WCF initialization failed. Check application Logs");
                Environment.Exit(-1);
            }
            await Task.Delay(5);
        }

        #endregion
    }
}
