using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace ExitMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private DateTime _Now = DateTime.Now;
        private List<int> _BaysNr = new List<int>();
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
            _eventAggregator.Subscribe(this);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;
        }

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            await ActivateItemAsync(new ExitViewModel(_windowManager, _eventAggregator, _BaysNr, _Controller));
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
                if (Global.Instance.CmdAppArgs.Length <= 0)
                {
                    throw new Exception(Global.Instance.LangTl("No parameter specified. Check application parameters"));
                }

                // numero del controller
                if (Global.Instance.CmdAppArgs.Length > 0)
                    if (int.TryParse(Global.Instance.CmdAppArgs[0], out int controller))
                        _Controller = controller;

                // numeri di baia gestiti (solo baie di ingresso)
                if (Global.Instance.CmdAppArgs.Length > 1)
                {
                    for (int i = 1; i < Global.Instance.CmdAppArgs.Length; i++)
                    {
                        if (int.TryParse(Global.Instance.CmdAppArgs[i], out int bayNr))
                            _BaysNr.Add(bayNr);
                    }
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

     

        public async Task HandleAsync(bool message, CancellationToken cancellationToken)
        {
            OnTelegramReceived?.Invoke(this, new GenericEventArgs(message));
        }


        public void CleanFields()
        {
            _eventAggregator.PublishOnUIThreadAsync("Clean");
        }

        #endregion
    }
}
