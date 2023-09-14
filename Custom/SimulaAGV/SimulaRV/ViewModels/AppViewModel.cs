using SimulaRV;
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
using CommsMgr;

namespace SimulaRV.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>.Collection.OneActive
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;
        private static AppViewModel _instance;
        private TrafficController _SelectedController;

        #endregion

        #region Properties

        public List<Screen> Screens { get; } = new List<Screen>();
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

        public TrafficController SelectedController
        {
            get { return _SelectedController; }
            set
            {
                _SelectedController = value;
                NotifyOfPropertyChange(() => SelectedController);
            }
        }
        public List<TrafficManager> Managers { get; set; }
        public ObservableCollection<TrafficManager> ManagerCollection { get; set; }
        public ObservableCollection<TrafficController> ControllerCollection { get; set; } = new ObservableCollection<TrafficController>();
        public ObservableCollection<BaseRuotineComponent> GenericManagerCollection { get; set; }
        public ObservableCollection<ScbState> ScbCollection { get; set; }
        public static AppViewModel Instance { get { return _instance; } }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = "SimulaRV";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Screens.Add(new HandlingPageViewModel(_windowManager, _eventAggregator));
            Screens.Add(new ShuttlePageViewModel(_windowManager, _eventAggregator));
            Screens.Add(new RazePageViewModel(_windowManager, _eventAggregator));
            Screens.Add(new AgvPageViewModel(_windowManager, _eventAggregator));

            ScbCollection = new ObservableCollection<ScbState>();

            _instance = this;

            LoadManagers();
        }

        #endregion

        #region ViewModel Override

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ActivateItem(Screens[0]);

            Global.Instance.OnDefaultMsgsManagement += Global_OnDefaultMsgsManagement;
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
                Global.Instance.OnDefaultMsgsManagement -= Global_OnDefaultMsgsManagement;

                Managers.ForEach(m => m.Dispose());
            }

            base.OnDeactivate(close);
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Aggiorno la data/ora
            Now = DateTime.Now;

            if (ScbCollection.Count <= 0) LoadConnectionInfo();
        }

        private void Global_OnDefaultMsgsManagement(object sender, GenericEventArgs e)
        {
            lock (ScbCollection)
            {
                if (ScbCollection == null || ScbCollection.Count <= 0) return;

                var scb = ScbCollection.FirstOrDefault(s => s.Id == (int)e.Arguments[0]);
                if (scb == null)
                {
                    ScbCollection.Clear();
                    return;
                }

                var state = (MsgDataDTO)e.Arguments[1];
                scb.NetworkState = state.QualityGood ? EChannelStates.Online : EChannelStates.Offline;
            }
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
                    WHERE MMG_Code LIKE '{ConfigurationManager.AppSettings["Manager"]}%'";
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
                _eventAggregator.PublishOnUIThread(false);

                Utils.Error(Global.Instance.LangTl("Cannot initialize traffic manager. Check application Logs"));
                return;
            }

            ManagerCollection = new ObservableCollection<TrafficManager>();
            NotifyOfPropertyChange(() => ManagerCollection);

            Managers.ForEach(m => ManagerCollection.Add(m));

            Managers.Where(x => x.ControllerCollection != null).SelectMany(x => x.ControllerCollection).ToList().ForEach(x => ControllerCollection.Add(x));
            SelectedController = ControllerCollection.FirstOrDefault();

            GenericManagerCollection = new ObservableCollection<BaseRuotineComponent>();
            NotifyOfPropertyChange(() => GenericManagerCollection);

            // Notifico ai ViewModel l'inizializzazione avvenuta
            _eventAggregator.PublishOnUIThread(true);
        }

        private void LoadConnectionInfo()
        {
            lock (ScbCollection)
            {
                try
                {
                    ScbCollection.Clear();

                    var scb = Global.Instance.CommsServerProxy.GetAllDvcBoardsStatus();

                    foreach (ScbDTO dto in scb)
                    {
                        ScbCollection.Add(new ScbState(dto.Id, dto.Code));
                    }
                }
                catch
                {
                    ScbCollection.Clear();
                }
            }
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            Managers.ForEach(m => m.Dispose());
            Managers = null;

            LoadManagers();
        }

        #endregion
    }
}

namespace SimulaRV
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
}
