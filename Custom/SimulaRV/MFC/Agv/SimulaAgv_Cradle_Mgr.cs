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
using mSwAgilogDll.SEW;
using static SimulaRV.SimulaAgv_Cradle_Tel;

namespace SimulaRV
{
    public class SimulaAgv_Cradle_Mgr : TrafficMgrBase
    {
        #region Members

        protected const int _pollingMillisec = 10000;
        protected DateTime _lastPollingTime;

        protected List<AgvMachine> _agv;        // Agv 

        #endregion

        #region Properties

        #endregion

        #region Constructor/Destructor

        public SimulaAgv_Cradle_Mgr(SqlConnection connection, DataRow row)
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
                var controller = c as SimulaAgv_Cradle_Ctr;
                if (controller != null)
                    controller.WcsIdentity = "WCS";
            });

            if (!LoadMachines()) return false;

            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;

            return true;
        }

        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            if (Global.Instance.Repetitions1Sec % 5 != 0)
            {
                RefreshMachine();
            }
        }
        protected virtual void RefreshMachine()
        {
            try
            {
                foreach (SEW_AGV agv in _agv)
                {
                    //agv.Refresh();
                    agv.AgvRequest.Refresh();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevels.Fatal);
                return;
            }
        }

        protected virtual bool LoadMachines()
        {
            try
            {
                _agv = new List<AgvMachine>();
                var agvs = AgvMachine.GetList(_conn);

                foreach (AgvMachine agv in agvs)
                {
                    // Considero solo le macchine che hanno un controller
                    // valido associato a questo manager
                    agv.Controller = _controllers.FirstOrDefault(c => c.Id == agv.AGV_CTR_Id);
                    agv.Cradles = HndModule.GetList<HndModule>(_conn, "WHERE [MOD_HMT_Code] = 'AGV' AND [MOD_PLN_Code] = '" + agv.AGV_Code + "'");
                    foreach (HndModule cradle in agv.Cradles)
                    {
                        cradle.Controller = _controllers.FirstOrDefault(c => c.Id == cradle.MOD_CTR_Id + 1000);
                    }
                    if (agv.Cradles[0].Controller != null)
                    {
                        _agv.Add(agv);

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevels.Fatal);
                return false;
            }
        }
        protected virtual async Task CheckIncomingTelegrams()
        {
            try
            {

                foreach (SimulaAgv_Cradle_Ctr controller in _controllers)
                {
                    var telegrams = controller.DequeueIncomingTelegrams().Cast<SimulaAgv_Cradle_Tel>().OrderBy(t => t.Timestamp);
                    _retrialsTelegram.Clear();



                    for (int i = 0; i < telegrams.Count(); i++)
                    {
                        SimulaAgv_Cradle_Tel telegram = telegrams.ElementAt(i);
                        ERetVal retVal = ERetVal.OK;

                        // Ignoro i telegrammi che non vanno gestiti
                        if (telegram.TelegramType == ETelegramTypes.PING ||
                            telegram.TelegramType == ETelegramTypes.NACK)
                        {
                            continue;
                        }

                        try
                        {
                            switch (telegram.TelegramType)
                            {
                                case ETelegramTypes.ACKT:
                                    //retVal = ACKT(telegram, agv); // Sull'ack del mio comando di scarico scarico passero da READY_TO_LOAD/UNLOAD a LOADING/UNLOADING
                                    break;

                                case ETelegramTypes.DONE:
                                    //retVal = DONE(telegram, agv);
                                    break;

                                default:
                                    Global.Instance.Log($"Unmanaged telegram {telegram}", LogLevels.Warning);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Global.Instance.Log($"Unmanaged exception while handling telegram {telegram}: {ex.Message}", LogLevels.Fatal);
                            retVal = ERetVal.FAIL;
                        }

                        if (retVal == ERetVal.RETRY)
                        {
                            if (!_retrialsTelegram.ContainsKey(telegram))
                                _retrialsTelegram.Add(telegram, 1);
                            else
                                _retrialsTelegram[telegram]++;

                            if (_retrialsTelegram[telegram] <= _retrialsMax)
                            {
                                await Task.Delay(_retrialsDelay);
                                i--;
                            }
                            else if (_retrialsTelegram.ContainsKey(telegram))
                            {
                                _retrialsTelegram.Remove(telegram);
                            }
                        }
                        else if (retVal == ERetVal.FAIL)
                        {
                            Global.Instance.Log($"Telegram {telegram.GetMessage()} FAILED to be managed", LogLevels.Fatal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Instance.Log($"Unmanaged exception while checking incoming telegrams: {ex.Message}", LogLevels.Fatal, "Handling_Mgr.CheckIncomingTelegrams");
            }
        }

        protected void PING()
        {
            foreach (SimulaAgv_Cradle_Ctr controller in _controllers)
            {
                SimulaAgv_Cradle_Tel telegram = new SimulaAgv_Cradle_Tel(ETelegramTypes.PING, controller.Code, "WCS");
                telegram.PingMillisec = controller.LastResponseDelay;

                controller.SendTelegram(telegram.GetMessage(), telegram.GetSignature(), true);
            }

            _lastPollingTime = DateTime.Now;
        }

        protected void DONE()
        {
            foreach (SEW_AGV agv in _agv)
            {
                if (agv.AgvRequest.MissionState == EAgvMissionState.LOADING)
                {
                    SimulaAgv_Cradle_Ctr cradle_controller = agv.Cradles[0].Controller as SimulaAgv_Cradle_Ctr;
                    SimulaAgv_Cradle_Tel telegram = new SimulaAgv_Cradle_Tel(ETelegramTypes.DONE, "SUMULATOR", agv.AGV_Code);
                    telegram.MissionID = (long)agv.AgvRequest.AGV_Mission;
                    ShuttleCradleCommand command = new ShuttleCradleCommand();
                    command.CommandResult = EMachineCommand_Results.LOAD_OK;
                    command.RackNum = agv.Part_data; // il part data corrisponde alla macchina da cui scaricare 
                    telegram.CradleCommands.Add(command);
                    if ((DateTime.Now - cradle_controller.LastSendTelegramTime).TotalSeconds > 10)
                    {
                        string retString = cradle_controller.SendTelegram(telegram.GetMessage(), "LOAD", true);
                        if (retString != null)
                            Global.Instance.Log($"Error to send LOAD telegram to {agv.AGV_Code}", LogLevels.Warning, "SimulaRV.DONE");
                        else
                        {
                            cradle_controller.LastSendTelegramTime = DateTime.Now;
                            ResetOngoingPollingTime();

                            if (!Global.Instance.SendCommandTrafficManager("PLC",
                                                             "LOAD",
                                                            new Dictionary<string, object>()
                                                            {
                                                                ["Mission"] = agv.AgvRequest.AGV_Mission
                                                            },
                                                             out string error))
                                Global.Instance.Log($"Error to send LOAD Command to Handling", LogLevels.Warning, "SimulaRV.DONE");
                        }

                    }
                }
                if (agv.AgvRequest.MissionState == EAgvMissionState.UNLOADING)
                {
                    SimulaAgv_Cradle_Ctr cradle_controller = agv.Cradles[0].Controller as SimulaAgv_Cradle_Ctr;
                    SimulaAgv_Cradle_Tel telegram = new SimulaAgv_Cradle_Tel(ETelegramTypes.DONE, "SUMULATOR", agv.AGV_Code);
                    telegram.MissionID = (long)agv.AgvRequest.AGV_Mission;
                    ShuttleCradleCommand command = new ShuttleCradleCommand();
                    command.CommandResult = EMachineCommand_Results.UNLOAD_OK;
                    command.RackNum = agv.Part_data; // il part data corrisponde alla macchina da cui scaricare 
                    telegram.CradleCommands.Add(command);
                    if ((DateTime.Now - cradle_controller.LastSendTelegramTime).TotalSeconds > 10)
                    {
                        string retString = cradle_controller.SendTelegram(telegram.GetMessage(), "UNLOAD", true);
                        if (retString != null)
                            Global.Instance.Log($"Error to send UNLOAD telegram to {agv.AGV_Code}", LogLevels.Warning, "SEW_Agv_Mgr.AgvTelegram");
                        else
                        {
                            cradle_controller.LastSendTelegramTime = DateTime.Now;
                            ResetOngoingPollingTime();
                        }
                    }
                }
            }
        }

        #endregion

        #region Manage Component

        protected override async Task<EComponentStates> ManageComponent()
        {
            // Timeout di esecuzione routine
            if (DateTime.Now.Subtract(_lastExec).TotalMilliseconds < _taskMillisec)
                return EComponentStates.Running;

            DONE();

            // Verifico i messaggi ricevuti dagli Agv
            await CheckIncomingTelegrams();

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
