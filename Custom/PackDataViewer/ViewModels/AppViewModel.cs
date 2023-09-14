using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using PackDataViewer.Views;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Threading;

namespace PackDataViewer.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>, IHandle<SnackbarMessage>, IHandle<bool>, IHandle<EAppViewCmds>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        public delegate void CustomEventHandler(object sender, GenericEventArgs e);
        public event CustomEventHandler OnSnackMessageRequested;

        #region Bound

        private DateTime _Now = DateTime.Now;
        private bool _LoadingIsVisible;

        #endregion

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

        public bool LoadingIsVisible
        {
            get { return _LoadingIsVisible; }
            set
            {
                if (_LoadingIsVisible == value) return;

                _LoadingIsVisible = value;
                NotifyOfPropertyChange(() => LoadingIsVisible);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!await InitApplicationParametersAsync())
                Environment.Exit(0);

            Global.Instance.App_Activated();
            await ActivateItemAsync(new PackDataViewModel(_windowManager, _eventAggregator));

            await base.OnInitializeAsync(cancellationToken);
        }

        #endregion

        #region Initialize

        private async Task<bool> InitApplicationParametersAsync()
        {
            try
            {
                if (Global.Instance.CmdAppArgs.Length < 2)
                {
                    await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Position parameter missing. Check configuration!"));
                    return false;
                }

                // Controller
                string handling = null;
                string traslo = null;

                if (Global.Instance.CmdAppArgs[0] != null)
                {
                    var parts = Global.Instance.CmdAppArgs[0].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    handling = parts[0];

                    if (parts.Length > 1)
                        traslo = parts[1];
                }

                if (!int.TryParse(handling, out int controller) || controller <= 0)
                {
                    await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Bad controller. Check configuration!"));
                    return false;
                }

                Common.Position = Global.Instance.CmdAppArgs[1];
                Common.ControllerID = controller;
                Common.ControllerIdentity = "PLC";

                if (traslo != null)
                {
                    if (!int.TryParse(traslo, out controller) || controller <= 0)
                    {
                        await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Bad Traslo controller. Check configuration!"));
                        return false;
                    }

                    Common.ControllerID_TR = controller;
                }
            }
            catch (Exception ex)
            {
                await Global.ErrorAsync(_windowManager, ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public async Task HandleAsync(SnackbarMessage message, CancellationToken cancellationToken)
        {
            OnSnackMessageRequested?.Invoke(this, new GenericEventArgs(message.Message));
            await Task.Delay(5);
        }

        public async Task HandleAsync(bool message, CancellationToken cancellationToken)
        {
            AppView.Instance.IsEnabled = message;
            await Task.Delay(5);
        }

        public async Task HandleAsync(EAppViewCmds message, CancellationToken cancellationToken)
        {
            if (message == EAppViewCmds.Close)
                await TryCloseAsync();
        }

        #endregion
    }

    public enum EAppViewCmds
    {
        Close = 0,
    }
}
