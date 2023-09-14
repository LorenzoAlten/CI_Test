using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using mSwAgilogDll;
using mSwAgilogDll.SEW;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using AgvMgr.Entites;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Threading;

namespace AgvMgr.ViewModels
{
    [Export(typeof(CfgManagerViewModel))]
    public class CfgManagerViewModel : Screen, IHandle<string>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _isEnabled = true;
        private Visibility _isAdmin = Visibility.Visible;

        private bool _IsLoading = true;
        private object _lockObj = new object();

        #region Bound

        private ObservableCollection<ConfigurationSEWViewModel> _agvsModel;

        #endregion

        #endregion

        #region Properties

        public Visibility IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                NotifyOfPropertyChange(() => IsAdmin);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
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

        public List<SEW_AGV> Agvs { get; private set; }

        public ObservableCollection<ConfigurationSEWViewModel> AgvsModel { get; set; } = new ObservableCollection<ConfigurationSEWViewModel>();
        #endregion

        #region Constructor

        [ImportingConstructor]
        public CfgManagerViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            BindingOperations.EnableCollectionSynchronization(AgvsModel, _lockObj);
        }

        #endregion

        #region ViewModel Override

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            LoadAgvs();
        }

        #endregion

        public async Task LoadAgvs()
        {
            IsLoading = true;

            await Task.Factory.StartNew(() =>
            {
                OnUIThread(() => AgvsModel.Clear());

                var agvEnt = new AgvEntities();
                var agvs = agvEnt.GetList();
                agvs = agvs.OrderBy(x => x.AGV_Code).ToList();

                foreach (AgvEntities agv in agvs)
                {
                    OnUIThread(() => AgvsModel.Add(new ConfigurationSEWViewModel(_windowManager, _eventAggregator, agv, true)));
                }
            });

            IsLoading = false;
        }

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            if (message == "Ricarica")
            {
                await LoadAgvs();
            }

            await Task.CompletedTask;
        }

        public void AddSew()
        {
            var agv = new AgvEntities();

            AgvsModel.Add(new ConfigurationSEWViewModel(_windowManager, _eventAggregator, agv, false));
        }
    }
}
