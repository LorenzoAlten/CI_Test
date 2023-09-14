using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel.Composition;

namespace WasteMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private DateTime _Now = DateTime.Now;
        private int _BayNr;
        private int _Controller;

        public delegate void TelegramEventHandler(object sender, GenericEventArgs e);
        public event TelegramEventHandler OnTelegramReceived;

        #endregion

        #region Properties

        public DateTime Now
        {
            get { return _Now; }
            private set
            {
                _Now = value;
                NotifyOfPropertyChange(() => Now);
            }
        }

        #endregion

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);

            Global.Instance.OnEvery100mSec += Global_OnEvery100mSec;
        }

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            await ActivateItemAsync(new WasteViewModel(_windowManager, _eventAggregator, _BayNr));

            await base.OnInitializeAsync(cancellationToken);
        }

        public override async Task ActivateItemAsync(Screen item, CancellationToken cancellationToken = default)
        {
            await base.ActivateItemAsync(item, cancellationToken);

            DisplayName = item.DisplayName;
        }

        #endregion

        #region Global Events

        private void Global_OnEvery100mSec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;
        }

        #endregion

        #region Private Methods

        private bool InitApplicationParameters()
        {
            try
            {
                if (Global.Instance.CmdAppArgs.Length <= 0)
                {
                    throw new Exception(Global.Instance.LangTl("No parameter specified. Check application parameters"));
                }
                if (Global.Instance.CmdAppArgs.Length > 1)
                    if (int.TryParse(Global.Instance.CmdAppArgs[0], out int controller))
                        _Controller = controller;

                if (Global.Instance.CmdAppArgs.Length > 0)
                    if (int.TryParse(Global.Instance.CmdAppArgs[1], out int bayNr))
                        _BayNr = bayNr;
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

        public async Task HandleAsync(bool message, CancellationToken cancellationToken)
        {
            OnTelegramReceived?.Invoke(this, new GenericEventArgs(message));
            await Task.CompletedTask;
        }

        public async Task CleanFields()
        {
            await _eventAggregator.PublishOnUIThreadAsync("Clean");
        }

        #endregion
    }
}
