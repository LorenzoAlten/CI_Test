using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;

namespace WhsViewer.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _isLoading;

        #endregion

        #region Properties

        public Screen Level { get; private set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Warehouse");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Level = new LevelViewModel(_windowManager, _eventAggregator);
        }

        #endregion

        #region ViewModel Override


        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            Orientation cellOrientation = (Orientation)Enum.Parse(typeof(Orientation), ConfigurationManager.AppSettings["CellOrientation"]);
            EAisleDirection aisleDirection = (EAisleDirection)Enum.Parse(typeof(EAisleDirection), ConfigurationManager.AppSettings["AisleDirection"]);

            IsLoading = true;
            await Task.Run(() =>
            {
                AgilogDll.WarehouseCellMgr whs = new AgilogDll.WarehouseCellMgr(
                    Global.Instance.ConnGlobal,
                    Global.Instance.DVC_Id,
                    Global.Instance.APP_Id,
                    Common.Warehouse,
                    cellOrientation,
                    aisleDirection);

            }).ContinueWith(antecedent =>
            {
                ActivateItemAsync(Level);
            });

            await base.OnInitializeAsync(cancellationToken);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
                AgilogDll.WarehouseCellMgr.Ptr.Dispose();
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Initialize

        private bool InitApplicationParameters()
        {
            try
            {
                if (Global.Instance.CmdAppArgs.Length <= 0)
                {
                    throw new Exception(Global.Instance.LangTl("No parameter specified. Check application parameters"));
                }

                Common.Warehouse = Global.Instance.CmdAppArgs[0];

                if (Global.Instance.CmdAppArgs.Length > 1 &&
                    int.TryParse(Global.Instance.CmdAppArgs[1], out int controller) &&
                    controller > 0)
                {
                    Common.StcController = controller;
                }
                else
                {
                    Common.StcController = -1;
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

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        public async Task HandleAsync(bool message, CancellationToken cancellationToken)
        {
            IsLoading = message;
        }

        #endregion
    }
}
