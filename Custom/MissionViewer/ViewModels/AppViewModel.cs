using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using mSwAgilogDll.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using AgilogDll.ViewModels;
using AgilogDll.Views;

namespace MissionViewer.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>
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
            DisplayName = "Agilog";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            string[] machines = null;

            if (Global.Instance.CmdAppArgs.Length > 0)
            {
                machines = Global.Instance.CmdAppArgs[0].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            Screens.Add(new MissionsSodicoViewModel(
                _windowManager,
                _eventAggregator,
                machines, true,
                mSwAgilogDll.EMisCfgStatesTypes.Undefined,
                AppSettings.Instance.ShowExceptionFields));

            Screens.Add(new BackupViewModel(_windowManager, _eventAggregator));
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Global.Instance.App_Activated();
            
            await base.OnInitializeAsync(cancellationToken);

            await ActivateItemAsync(Screens.First());
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

        private async void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;

            if (Global.Instance.Repetitions1Sec % 10 == 0)
            {
                await _eventAggregator.PublishOnUIThreadAsync("REFRESH");
            }
        }

        #endregion
    }
}
