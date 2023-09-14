using Caliburn.Micro;
using mSwDllWPFUtils;
using System.ComponentModel.Composition;
using mSwAgilogDll;
using mSwAgilogDll.SEW;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AgvMgr.ViewModels
{
    [Export(typeof(MissionDetailsViewModel))]
    public class MissionDetailsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Properties
        public SEW_AGV Agv { get; set; }
        public AgvRequest AgvRequest { get; set; }
        public List<EAgvMissionState> AgvMissionStateList { get { return Enum.GetValues(typeof(EAgvMissionState)).Cast<EAgvMissionState>().ToList(); } }
        
        #endregion

        #region Constructor

        [ImportingConstructor]
        public MissionDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SEW_AGV agv)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            Agv = agv;
            AgvRequest = (AgvRequest)agv.AgvRequest.Clone();

            DisplayName = string.Empty;
        }

        #endregion

        #region Public Method
        public void Salva()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                             "UPDATE_AGV_MISSION",
                                            new Dictionary<string, object>()
                                            {
                                                ["MissionState"] = (int)AgvRequest.MissionState,
                                                ["AGV_Mission"] = AgvRequest.AGV_Mission.HasValue ? AgvRequest.AGV_Mission.Value : string.Empty,
                                                ["AGV_Loaded"] = AgvRequest.AGV_Loaded,
                                                ["AGV_Code"] = Agv.AGV_Code
                                            },
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }
        #endregion
    }
}
