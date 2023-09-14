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

namespace ToyotaClientWebServiceSoapMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private DateTime _Now = DateTime.Now;
       

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
            _eventAggregator.SubscribeOnPublishedThread(this);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;
        }

        #region ViewModel Override+

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            await ActivateItemAsync(new ToyotaClientServiceSoapViewModel(_windowManager, _eventAggregator));
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


        public void CleanFields()
        {
            _eventAggregator.PublishOnUIThreadAsync("Clean");
        }

        #endregion
    }
}
