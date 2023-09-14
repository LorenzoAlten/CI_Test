using Caliburn.Micro;
using mSwAgilogDll.ViewModels;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace WhsMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>.Collection.OneActive
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;

        #endregion

        #region Properties

        public List<Screen> Screens { get; } = new List<Screen>();

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

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Stocks");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Screens.Add(new AgilogDll.ViewModels.ItemsViewModel(_windowManager, _eventAggregator, false));
            Screens.Add(new AgilogDll.ViewModels.UdcsViewModel(_windowManager, _eventAggregator));
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Global.Instance.App_Activated();

            await base.OnInitializeAsync(cancellationToken);

            await ActivateItemAsync(Screens[0]);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;
        }

        #endregion
    }
}
