using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>, IHandle<EUserOperations>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private RunUtils _utils;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;
        private int _BayNr;

        private bool _isEnabled;

        #endregion

        #region Properties

        //public Screen Orders { get; private set; }

        public bool LoadingIsVisible
        {
            get { return _LoadingIsVisible; }
            private set
            {
                _LoadingIsVisible = value;
                NotifyOfPropertyChange(() => LoadingIsVisible);
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

        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = "Agilog";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            //Orders = new OrdersViewModel(_windowManager, _eventAggregator);

            IsEnabled = true;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            await ActivateItemAsync(new OrdersViewModel(_windowManager, _eventAggregator, _BayNr));

            await base.OnInitializeAsync(cancellationToken);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        public override async Task ActivateItemAsync(Screen item, CancellationToken cancellationToken = default)
        {
            await base.ActivateItemAsync(item, cancellationToken);

            DisplayName = item.DisplayName;
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;
        }

        #endregion

        #region Private Methods

        private bool InitApplicationParameters()
        {
            try
            {
                if (Global.Instance.CmdAppArgs.Length > 0 &&
                    int.TryParse(Global.Instance.CmdAppArgs[0], out int bayNr))
                {
                    _BayNr = bayNr;
                }
                else
                {
                    _BayNr = 0;
                    _utils.LoadOuputBays();
                }
            }
            catch (Exception ex)
            {
                Utils.ShowException(ex);
                return false;
            }

            return true;
        }

        #endregion

        #region Public Methods

        public async Task HandleAsync(EUserOperations message, CancellationToken cancellationToken)
        {
            if (message > EUserOperations.Noop)
            {
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
            }
            await Task.Delay(5);
        }

        #endregion
    }

    public enum EUserOperations
    {
        Noop,
        Start,
        Stop,
        Prepare,
    }
}
