using mSwAgilogDll;
using mSwDllMFC;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMgr
{
    public class ShuttleState : BaseBindableObject
    {
        /*
        #region Members

        protected string _shuttle;
        protected EChannelStates _networkState;
        protected int _battery;
        protected int _batteryLevel;

        #endregion

        #region Properties

        public int ControllerID { get; set; }

        public string Shuttle
        {
            get { return _shuttle; }
            set { SetProperty(ref _shuttle, value); }
        }

        public EChannelStates NetworkState
        {
            get { return _networkState; }
            set { SetProperty(ref _networkState, value); }
        }

        public int Battery
        {
            get { return _battery; }
            set { SetProperty(ref _battery, value); }
        }

        #endregion

        #region Constructor/Destructor

        public ShuttleState(int controllerID, ControllerInfoDTO dto)
        {
            ControllerID = controllerID;
            Shuttle = dto.Code;
            SetNetworkState(dto);
            SetBatteryLevel(dto);
        }

        #endregion

        public void SetNetworkState(ControllerInfoDTO dto)
        {
            if (dto == null)
                NetworkState = EChannelStates.Unknown;
            else
                NetworkState = (EChannelStates)dto.NetworkState;
        }

        public void SetBatteryLevel(ControllerInfoDTO dto)
        {
            if (dto == null)
            {
                Battery = 1;
                return;
            }

            // La batteria và da 46 a 54 V
            var battery = dto.PublicPropertys.FirstOrDefault(p => p.Name == EAutomhaCtr_Variables.BatteryLevel.ToString());
            if (battery == null || battery.Value == null)
                Battery = 1;
            else
            {
                decimal batteryLevel = (decimal)battery.Value;
                if (batteryLevel > 52) Battery = 3;
                else if (batteryLevel > 49) Battery = 2;
                else Battery = 1;
            }
        }
        */
    }
}
