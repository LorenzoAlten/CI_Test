using Caliburn.Micro;
using mSwDllWPFUtils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using mSwAgilogDll;
using System.Windows.Input;
using System.Windows.Controls;
using mSwAgilogDll.SEW;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using AgvMgr.Entites;
using System.Threading;
using AgvMgr.AppData;
using System.Threading.Tasks;

namespace AgvMgr.ViewModels
{
    [Export(typeof(AgvManagerViewModel))]
    public class AgvManagerViewModel : Conductor<Screen>.Collection.AllActive
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Properties

        public List<SEW_AGV> Agvs { get; private set; }

        public MissionsViewModel MissionGrid { get; private set; }

        public ObservableCollection<SEWAgvViewModel> AgvsModel { get; private set; } = new ObservableCollection<SEWAgvViewModel>();

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AgvManagerViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            
            // Nella parte superiore della pagina carico la lista missioni che è comodo averla sott occhio assieme allo stato Agv
            MissionGrid = new MissionsViewModel(_windowManager, _eventAggregator);
            MissionGrid.ConductWith(this);
        }

        #endregion

        #region ViewModel Override

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            AgvsModel.Clear();

            foreach (SEW_AGV agv in Common.Instance.Agvs)
            {
                var agvModel = new SEWAgvViewModel(_windowManager, _eventAggregator, agv);
                agvModel.ConductWith(this);
                AgvsModel.Add(agvModel);
            }

            return base.OnInitializeAsync(cancellationToken);
        }

        #endregion

        #region Initialize

        #endregion

        #region Global Events

        #endregion

        #region Private Methods

        #endregion        
    }
}
