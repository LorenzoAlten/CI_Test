using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using mSwDllMFC;
using mSwAgilogDll.Errevi;

namespace SimulaRV
{
    public class SimulaHdl_Mgr : TrafficMgrBase
    {
        #region Members

        protected const int _pollingMillisec = 10000;
        protected DateTime _lastPollingTime;

        #endregion

        #region Properties

        #endregion

        #region Constructor/Destructor

        public SimulaHdl_Mgr(SqlConnection connection, DataRow row)
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
                var controller = c as SimulaHdl_Ctr;
                if (controller != null)
                    controller.WcsIdentity = "WCS";
            });

            return true;
        }

        protected void PING()
        {
            foreach (SimulaHdl_Ctr controller in _controllers)
            {
                SimulaHdl_Tel telegram = new SimulaHdl_Tel(ETelegramTypes.PING, controller.Code, "WCS");
                telegram.PingMillisec = controller.LastResponseDelay;

                controller.SendTelegram(telegram.GetMessage(), telegram.GetSignature(), true);
            }

            _lastPollingTime = DateTime.Now;
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
                // Invio il polling: lo gestisco qui e non a livello di messaggio
                // continuo di controller perché devo gestire la variabile del
                // tempo di risposta ultimo rilevato
                if (DateTime.Now.Subtract(_lastPollingTime).TotalMilliseconds >= _pollingMillisec)
                {
                    PING();
                }
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
