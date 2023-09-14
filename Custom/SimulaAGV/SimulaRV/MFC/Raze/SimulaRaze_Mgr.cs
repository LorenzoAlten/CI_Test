using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mSwDllMFC;
using mSwAgilogDll.Errevi;

namespace SimulaRV
{
    public class SimulaRaze_Mgr : TrafficMgrBase
    {
        #region Members

        protected const int _pollingMillisec = 10000;
        protected DateTime _lastPollingTime;

        #endregion

        #region Properties

        #endregion

        #region Constructor/Destructor

        public SimulaRaze_Mgr(SqlConnection connection, DataRow row)
            : base(connection, row)
        {
            _lastPollingTime = DateTime.MinValue;
        }

        #endregion

        #region Public Metohds

        #endregion

        #region Protected Methods

        protected override bool Init()
        {
            bool retVal = base.Init();
            if (!retVal) return false;

            _controllers.ForEach(c =>
            {
                var controller = c as SimulaRaze_Ctr;
                if (controller != null)
                    controller.WcsIdentity = "WCS";
            });

            return true;
        }

        #endregion

        #region Manage Component

        protected override async Task<EComponentStates> ManageComponent()
        {
            // Timeout di esecuzione routine
            if (DateTime.Now.Subtract(_lastExec).TotalMilliseconds < _taskMillisec)
                return EComponentStates.Running;

            try
            {
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
                return EComponentStates.Error;
            }
            finally
            {
                _lastExec = DateTime.Now;
            }

            return await base.ManageComponent();
        }

        #endregion
    }
}
