using Caliburn.Micro;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public List<TrafficManager> Managers { get; } = new List<TrafficManager>();
        public ObservableCollection<TrafficManager> ManagerCollection { get; } = new ObservableCollection<TrafficManager>();
        public ObservableCollection<TrafficController> ControllerCollection { get; } = new ObservableCollection<TrafficController>();
        public ObservableCollection<BaseRuotineComponent> GenericManagerCollection { get; } = new ObservableCollection<BaseRuotineComponent>();
        public ObservableCollection<ScbState> ScbCollection { get; } = new ObservableCollection<ScbState>();
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
            Screens.Add(new CommandPageViewModel(_windowManager, _eventAggregator));
            Screens.Add(new AgvPageViewModel(_windowManager, _eventAggregator));

            ScbCollection = new ObservableCollection<ScbState>();

            _instance = this;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            await LoadManagersAsync();

            await ActivateItemAsync(Screens[0]);

            Global.Instance.OnDefaultMsgsManagement += Global_OnDefaultMsgsManagement;
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
                Global.Instance.OnDefaultMsgsManagement -= Global_OnDefaultMsgsManagement;

                Managers.ForEach(m => m.Dispose());
            }
            
            await base.OnDeactivateAsync(close, cancellationToken);
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

                var state = (mSwDllGrpcCommon.MsgData)e.Arguments[1];
                scb.NetworkState = state.QualityGood ? EChannelStates.Online : EChannelStates.Offline;
            }
        }

        #endregion

        #region Private Methods

        private async Task LoadManagersAsync()
        {
            IsLoading = true;
            Managers.Clear();

            await Task.Run(() =>
            {
                var query = $@"
                    SELECT * FROM [MFC_MANAGERS]
                    WHERE MMG_Owner = 'SimulaRV'";
                var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);
                if (dt == null || dt.Rows.Count <= 0) return;

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

            Managers.ForEach(m => ManagerCollection.Add(m));

            Managers.Where(x => x.ControllerCollection != null).SelectMany(x => x.ControllerCollection).ToList().ForEach(x => ControllerCollection.Add(x));
            SelectedController = ControllerCollection.FirstOrDefault();

            // Notifico ai ViewModel l'inizializzazione avvenuta
            await _eventAggregator.PublishOnUIThreadAsync(true);
        }

        private void LoadConnectionInfo()
        {
            lock (ScbCollection)
            {
                try
                {
                    ScbCollection.Clear();

                    var scbList = Global.Instance.CommsServiceClient.GetAllDvcBoardsStatus(new Google.Protobuf.WellKnownTypes.Empty());

                    foreach (mSwDllGrpcCommon.Scb dto in scbList.Scbs)
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
            Managers.Clear();

            LoadManagersAsync();
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
