using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using mSwDllMFC;

namespace Movitrans
{
    public class Movitrans_Mgr : TrafficManager
    {
        #region Members

        protected int _taskMillisec = 500;
        protected int _controller;
        protected bool _EnSavingEanbled;
        protected int _EnSavingWait;
        protected short _EnSavingOutputPower;

        protected short _OutputPower = 100;

        #endregion

        #region Properties

        public bool EnSavingEnabled
        {
            get { return _EnSavingEanbled; }
            set
            {
                Parameter par = ParamManager.Parameters.FirstOrDefault(p => p.PAR_Code == "MOV_EnSavingEnabled");
                if (par == null) return;
                par.PAR_Value = value ? "1" : "0";
                if (!par.Save()) return;

                _EnSavingEanbled = value;
            }
        }

        public int EnSavingWait
        {
            get { return _EnSavingWait; }
            set
            {
                if (value < 0) value = 0;

                Parameter par = ParamManager.Parameters.FirstOrDefault(p => p.PAR_Code == "MOV_Minutes");
                if (par == null) return;
                par.PAR_Value = value.ToString();
                if (!par.Save()) return;

                _EnSavingWait = value;
            }
        }

        public short EnSavingOutputPower
        {
            get { return _EnSavingOutputPower; }
            set
            {
                if (value > _OutputPower) value = _OutputPower;

                if (value < 0) value = 0;

                Parameter par = ParamManager.Parameters.FirstOrDefault(p => p.PAR_Code == "MOV_LowOutputPower");
                if (par == null) return;
                par.PAR_Value = value.ToString();
                if (!par.Save()) return;

                _EnSavingOutputPower = value;
            }
        }

        public List<SEWMovitrans> SewMovitrans { get; set; } = new List<SEWMovitrans>();

        #endregion

        #region Constructor/Destructor

        public Movitrans_Mgr(SqlConnection connection, DataRow row)
            : base(connection, row)
        {
            _taskMillisec = 500;
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
        }

        #endregion

        #region Public Metohds

        #endregion

        #region Protected Methods

        protected override bool Init()
        {
            bool retVal = base.Init();

            if (!retVal) return false;

            // Inizializzo la lettura dei parametri
            ParamManager.Init(Global.Instance.ConnGlobal);

            string query = $@"SELECT * 
                            FROM MFC_MOVITRANS
                            WHERE MOV_CTR_Id IN ({string.Join(", ", _controllers.Select(x => x.Id).ToList())})";

            DataTable dt = DbUtils.ExecuteDataTable(query, _conn);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SEWMovitrans movitrans = new SEWMovitrans(dr.GetValue("MOV_Code"))
                    {
                        ControllerId = dr.GetValueI("MOV_CTR_Id")
                    };

                    SewMovitrans.Add(movitrans);
                }
            }

            _EnSavingEanbled = ParamManager.GetValueI("MOV_EnSavingEnabled") == 1;
            _EnSavingWait = ParamManager.GetValueI("MOV_Minutes");
            _EnSavingOutputPower = (short)ParamManager.GetValueI("MOV_LowOutputPower");

            return true;
        }

        protected virtual async Task CheckIncomingTelegrams()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Global.Instance.Log($"Unmanaged exception while checking incoming telegrams: {ex.Message}", LogLevels.Fatal, "MOVITRANS", "CheckIncomingTelegrams");
            }
        }

        public override string ExecCommand(string command, Dictionary<string, object> parameters)
        {
            return null;
        }

        #endregion

        #region Send Telegrams

        protected virtual void SendCommands()
        {
            try
            {
                foreach (TrafficController controller in _controllers)
                {
                    Movitrans_IN_Tel telegram = new Movitrans_IN_Tel();
                    telegram.Receive_Port = (short)controller.Channels[0].LocalPort;
                    foreach (SEWMovitrans mov in SewMovitrans.Where(x => x.ControllerId == controller.Id))
                    {
                        telegram.SendData.Add(new Movitrans_IN_Tel.MOV_UDPData());

                        if (mov.Enabled)
                        {
                            telegram.SendData.LastOrDefault().Control_Word = MOV_Control_Word.Movitrans_Enable;

                            if (mov.ResetRequest)
                                telegram.SendData.LastOrDefault().Control_Word |= MOV_Control_Word.Movitrans_Reset;
                        }
                        else
                        {
                            telegram.SendData.LastOrDefault().Control_Word = 0;
                        }

                        telegram.SendData.LastOrDefault().SetPoint = mov.SetPoint;
                    }

                    var telegramma = telegram.GetMessage();
                    controller.SendTelegram(telegramma, "READ", false);
                    SewMovitrans.Where(x => x.ControllerId == controller.Id).ToList().ForEach(sm => sm.ResetRequest = false);
                }
            }
            catch (Exception ex)
            {
                Global.Instance.Log($"Unmanaged exception during Agv reading: {ex.Message}", LogLevels.Fatal, "MOVITRANS", "SendCommands");
            }
        }

        #endregion

        #region Manage Component

        protected override async Task<EComponentStates> ManageComponent()
        {
            if (_controllers == null ||
                _controllers.Any(c => !c.InitComplete)) return EComponentStates.Starting;

            // Timeout di esecuzione routine
            if (DateTime.Now.Subtract(_lastExec).TotalMilliseconds < _taskMillisec)
                return EComponentStates.Running;
            try
            {
                if (EnSavingEnabled)
                {
                    string query = $@"
                                DECLARE @DATEDIFF int

								SET @DATEDIFF = 0

								IF NOT EXISTS (SELECT [MIS_Id] FROM [MIS_MISSIONS] WITH (READUNCOMMITTED))
								BEGIN
									DECLARE @MIS_L1_State nvarchar(50)
									DECLARE @MIS_L2_State nvarchar(50)
									DECLARE @BCK_Time datetime

									SELECT TOP 1 @MIS_L1_State = [MIS_L1_MST_Code], @MIS_L2_State = [MIS_L2_MST_Code], @BCK_Time = [BCK_Time]
									FROM [BCK_MISSIONS] WITH (READUNCOMMITTED)
									ORDER BY [BCK_Id] DESC

									IF @MIS_L1_State = 'END' AND @MIS_L2_State = 'END'
									BEGIN
										SELECT @DATEDIFF = DATEDIFF(minute, @BCK_Time, GETDATE()) 
									END
								END

                                SELECT @DATEDIFF";

                    var diffTime = DbUtils.ExecuteScalar(query, _conn);

                    if (diffTime != null && diffTime != DBNull.Value)
                    {
                        foreach (SEWMovitrans mov in SewMovitrans)
                        {
                            // Se il movitrans è abilitato e ha ricevuto missioni negli ultimi 5 minuti allora gli do il 100%, altrimenti il valore da cfg param
                            if (mov.Enabled && (int)diffTime < EnSavingWait)
                            {
                                mov.SetPoint = _OutputPower; // Se il movitrans è abilitato gli do il 100 %
                            }
                            else if (mov.Enabled && (int)diffTime > EnSavingWait)
                            {
                                mov.SetPoint = EnSavingOutputPower;
                            }
                        }
                    }
                }
                else
                {
                    // Se non è abilitato il risparmio energetico il setpoint è fisso
                    foreach (SEWMovitrans mov in SewMovitrans)
                    {
                        if (mov.Enabled) mov.SetPoint = _OutputPower;
                    }
                }

                // Invio i messaggi
                SendCommands();
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal, "MOVITRANS", "ManageComponent");
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

