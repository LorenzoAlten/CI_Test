using Caliburn.Micro;
using mSwDllGrpc;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AstarMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;
        private string _astarServerAddress;
        //public List<Screen> Screens { get; } = new List<Screen>();

        #endregion

        #region Properties

        public static AppViewModel Instance { get; private set; }
        public Screen Jobs { get; private set; }
        public Screen JobsBlueYonder { get; private set; }
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

        public string AstarServerAddress
        {
            get { return _astarServerAddress; }
            set
            {
                _astarServerAddress = value;
                NotifyOfPropertyChange(() => AstarServerAddress);
            }
        }

        public ObservableCollection<TrafficManager> ManagerCollection { get; set; }
        public ObservableCollection<TrafficController> ControllerCollection { get; set; } = new ObservableCollection<TrafficController>();
        public ObservableCollection<BaseRuotineComponent> GenericManagerCollection { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Astar Manager");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Jobs = new JobsViewModel(_windowManager, _eventAggregator);
            Jobs.ConductWith(this);
            //Screens.Add(new ExotecTestViewModel(_windowManager, _eventAggregator));
            //Screens.Add(new JobsViewModel(_windowManager, _eventAggregator));
            //Jobs.ConductWith(this);           
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            await LoadManagersAsync();
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;

                Shared.Managers.ForEach(m => m.Dispose());
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Aggiorno la data/ora
            Now = DateTime.Now;
        }

        #endregion

        #region Attached Events

        public void OnWindowActivated()
        {
            Global.Instance.App_Activated();
        }

        #endregion

        #region Protected Methods

        private async Task LoadManagersAsync()
        {
            IsLoading = true;
            Shared.Managers = null;
            List<BaseRuotineComponent> managers = null;

            await Task.Run(() =>
            {
                DateTime now = DateTime.Now;

                managers = TrafficManager.GetList((SqlConnection)Global.Instance.ConnGlobal, Global.Instance.DVC_Id, AppDomain.CurrentDomain.FriendlyName);
                while (managers.Any(m => !m.InitComplete || (m is TrafficManager && (m as TrafficManager).ControllerCollection.Any(c => !c.InitComplete))))
                {
                    if (DateTime.Now.Subtract(now).TotalSeconds > 20)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Cannot initialize AstarManager. Check application Logs"));
                            Environment.Exit(0);
                        });
                    }
                    Task.Delay(10);
                }
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });

            if (managers == null || managers.Count <= 0)
            {
                // Notifico ai ViewModel l'inizializzazione fallita
                await _eventAggregator.PublishOnUIThreadAsync(false);
                await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Cannot initialize AstarManager. Check application Logs"));
                return;
            }

            Shared.Managers = managers.Where(m => m is TrafficManager).Select(m => m as TrafficManager).ToList();

            ManagerCollection = new ObservableCollection<TrafficManager>();
            NotifyOfPropertyChange(() => ManagerCollection);

            Shared.Managers.ForEach(m => ManagerCollection.Add(m));

            GenericManagerCollection = new ObservableCollection<BaseRuotineComponent>();
            NotifyOfPropertyChange(() => GenericManagerCollection);
            managers.Except(Shared.Managers).ToList().ForEach(m => GenericManagerCollection.Add(m));

            // Notifico ai ViewModel l'inizializzazione avvenuta
            await _eventAggregator.PublishOnUIThreadAsync(true);

            // Lancio il servizio Grpc
            AstarServiceMgr mgr = new AstarServiceMgr(Shared.Managers.First() as IRestManager);
            mgr.OpenAstarServer();

            AstarServerAddress = mgr.AstarServerAddress;
        }

        #endregion
    }
}
