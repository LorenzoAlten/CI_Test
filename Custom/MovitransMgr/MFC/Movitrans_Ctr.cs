using AgilogDll;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Movitrans
{
    public class Movitrans_Ctr : TrafficController
    {
        #region Members

        protected DateTime _lastSendTime;
        protected DateTime _lastRecTime;
        protected DateTime _lastPingRecTime;
        protected TrafficChannel _pingChannel;
        protected int _pingTimeout = 10;
        protected int _recTimeout = 10;
        protected double _delayTime;

        protected bool FirstTelegramReceived = true;

        private List<AgvMovitrans> _agvMovitrans;

        public event RoutineEventHandler OnAGVUpdate;
        public event RoutineEventHandler OnAGVUserUpdate;

        #endregion

        #region Properties

        public int LastResponseDelay
        {
            get
            {
                if (_lastSendTime > _lastRecTime) return _delayTime > int.MaxValue ? int.MaxValue : (int)_delayTime;

                _delayTime = _lastRecTime.Subtract(_lastSendTime).TotalMilliseconds;
                return _delayTime > int.MaxValue ? int.MaxValue : (int)_delayTime;
            }
        }

        #endregion

        #region Constructor/Destructor

        public Movitrans_Ctr(SqlConnection connection, TrafficManager handling, DataRow row)
            : base(connection, handling, row)
        {
            Telegram_FixedLength = 0;

            _lastSendTime = DateTime.MinValue;
            _lastRecTime = DateTime.MinValue;
            _lastPingRecTime = DateTime.Now;
            _delayTime = 0;

            _agvMovitrans = BaseBindableDbEntity.GetList<AgvMovitrans>(DbUtils.CloneConnection(_conn));
        }

        #endregion

        #region Public Metohds

        #endregion

        #region Protected Method for WCF Variable

        protected override string[] RefreshVariables(Telegram telegram)
        {
            List<string> names = new List<string>();
            var tel = telegram as Movitrans_OUT_Tel;
            //AGV.CTR_ID = Id;
            //AGV.InitFromTelegram(tel);
            /*
            names.Add(Id.ToString());
            Agv.ID = tel.ID;
            names.Add(tel.ID.ToString());
            Agv.Part_data = tel.Part_data;
            names.Add(tel.Part_data.ToString());
            */
            // Tentativo di passare direttamente l'oggetto come Public Variable
            // serializzandolo. (vedi commento nel metodo SetPublicVariablesReadings) 
            /*
            XmlSerializer serializer = new XmlSerializer(typeof(SEW_Agv));
            StringBuilder result = new StringBuilder();
            using (var writer = XmlWriter.Create(result))
            {
                serializer.Serialize(writer, Agv);  /// serialize              
            }
            
            SetProperty("SEW_Agv", result.ToString());
            */

            //PublicVariables = AGV.GetPublicVariables();
            int i = 0;
            foreach (var mov_tel in tel.ReceivedData)
            {
                if (i >= ((Movitrans_Mgr)_manager).SewMovitrans.Count) continue;

                SEWMovitrans mov = ((Movitrans_Mgr)_manager).SewMovitrans[i];
                i++;

                mov.ErrorCode = mov_tel.ErrorCode;
                mov.OutputPower = (mov_tel.OutputPower / 1000);
                mov.Status = mov_tel.Status_Word;
                mov.Utilization = mov_tel.Utilization;
                mov.WarningCode = mov_tel.WarningCode;
                mov.Temperature = mov_tel.Temperature;

                if (FirstTelegramReceived)
                {
                    if (mov.Status.HasFlag(MOV_Status_Word.Remote_device_Connected))
                        mov.Enabled = true;
                }

                // Riempio la tabella con i dati del movitrans
                AgvMovitrans agvMov = _agvMovitrans.FirstOrDefault(x => x.MOV_Code == mov.Code);

                if (agvMov != null)
                {
                    AgvMovitrans oldAgvMov = agvMov;

                    agvMov.MOV_OutputPower = mov_tel.OutputPower;
                    agvMov.MOV_Status = (int?)mov.Status;
                    agvMov.MOV_Utilization = mov.Utilization;
                    agvMov.MOV_Temperature = mov.Temperature;

                    if (oldAgvMov.MOV_OutputPower != agvMov.MOV_OutputPower
                        || oldAgvMov.MOV_Status != agvMov.MOV_Status
                        || oldAgvMov.MOV_Utilization != agvMov.MOV_Utilization
                        || oldAgvMov.MOV_Temperature != agvMov.MOV_Temperature)
                    {
                        agvMov.Update();
                    }
                }

                if (FirstTelegramReceived) continue;

                try
                {
                    if (mov.Status.HasFlag(MOV_Status_Word.Switch_Cabinet_Enabled) && !mov.Status.HasFlag(MOV_Status_Word.Remote_device_Connected))
                    {
                        agvMov.SEW_Error = $"Alarm from SEW - Remote Device Not Connected";
                    }
                    else if (!mov.Status.HasFlag(MOV_Status_Word.Error) && (mov.WarningCode == 0 || mov.WarningCode == (short)MOV_Warning.Abilitazione))
                    {
                        agvMov.SEW_Error = "";
                    }
                    else
                    {
                        string errorCode = GetMOV_Error(mov.ErrorCode);
                        string warningCode = GetMOV_Warning(mov.WarningCode);

                        if (string.IsNullOrEmpty(errorCode) && string.IsNullOrEmpty(warningCode))
                        {
                            agvMov.SEW_Error = $"Alarm from SEW";
                        }
                        else if (string.IsNullOrEmpty(errorCode))
                        {
                            agvMov.SEW_Error = $"Alarm from SEW - WarningCode: {warningCode}";
                        }
                        else if (string.IsNullOrEmpty(warningCode))
                        {
                            agvMov.SEW_Error = $"Alarm from SEW - ErrorCode: {errorCode}";
                        }
                        else
                        {
                            agvMov.SEW_Error = $"Alarm from SEW - ErrorCode: {errorCode} - WarningCode: {warningCode}";
                        }
                    }

                    if (agvMov.LastSEW_ErrorLogged == null || agvMov.SEW_Error != agvMov.LastSEW_ErrorLogged)
                    {
                        try
                        {
                            var row = INSERT_HAL_Agv(agvMov.MOV_Code, agvMov.SEW_Error, 0, _conn);

                            // Valuto l'esito della query
                            int rc = row.GetValueI("RetVal");
                            string error = row.GetValue("Error");

                            if (rc != 0)
                            {
                                throw new Exception("RetVal not Ok");
                            }

                            agvMov.LastSEW_ErrorLogged = agvMov.SEW_Error;
                        }
                        catch (Exception ex)
                        {
                            Global.Instance.Log($"An error occurred while trying to execute INSERT_HAL_Agv: {ex.Message}", LogLevels.Fatal, "Movitrans_Ctr", "RefreshVariables");
                        }
                    }
                }
                catch { }
            }

            FirstTelegramReceived = false;

            //OnAGVUpdate?.Invoke(this, new GenericEventArgs(tel));

            return names.ToArray();
        }

        #endregion

        #region Override Methods

        protected override void OnMsgSent(TrafficChannel sender, string message)
        {
            if (_pingChannel == null) _pingChannel = sender;

            // Se non sto ricevendo messaggi da un tempo configurato,
            // devo resettare la comunicazione sul canale apposito
            if (DateTime.Now.Subtract(_lastPingRecTime).TotalSeconds > _pingTimeout && _pingChannel != null)
            {
                _pingChannel.ChannelState = EChannelStates.Offline; // ATTENZIONE. C'è già un timeout nel controller generico che mette offline 
                                                                    // il canale che lavora su qualsiasi ricezione.
                _lastPingRecTime = DateTime.Now;

                string log = DateTime.Now.ToString("HH:mm:ss.FFF") + ";" + _pingChannel.ToString() + ";(RECEIVE PING ERROR) '_lastPingRecTime > _pingTimeout'";
                _manager.LogsFileUpdate($"{Code} LOG_SOCKET.csv", new List<string> { log });
            }
        }

        protected override void OnMsgReceived(TrafficChannel sender, Telegram telegram)
        {
            // Memorizzo l'orario di ultima ricezione messaggio
            _lastRecTime = DateTime.Now;
            _lastPingRecTime = DateTime.Now;

            base.OnMsgReceived(sender, telegram);
        }

        protected override async Task<EComponentStates> ManageComponent()
        {
            if (_channels == null || _channels.Count < 1) return EComponentStates.Running;

            try
            {
                // Verifico lo stato dei canali e, se necessario,
                // reinizializzo la comunicazione
                foreach (TrafficChannel channel in _channels)
                {
                    bool renew = channel.ChannelState <= EChannelStates.Offline;
                    renew |= channel.ChannelState == EChannelStates.Connecting &&
                             DateTime.Now.Subtract(channel.ChannelStateChanged).TotalMilliseconds > _channelRenewalTimeOut;

                    renew |= channel.ApplyReceiveDataTimeout && channel.DataTimeoutExpired;
                    bool timeOutScaduto = channel.ApplyReceiveDataTimeout && channel.DataTimeoutExpired;

                    if (channel.IsEnabled && renew)
                    {
                        string log;
                        if (timeOutScaduto)
                            log = DateTime.Now.ToString("HH:mm:ss.FFF") + ";" + channel.ToString() + ";(OPEN SOCKET) Riapertura socket per TimeOut ricezione";
                        else
                            log = DateTime.Now.ToString("HH:mm:ss.FFF") + ";" + channel.ToString() + ";(OPEN SOCKET) Riapertura socket per channel offline";
                        _manager.LogsFileUpdate($"{Code} LOG_SOCKET.csv", new List<string> { log });

                        if (channel is IWebServiceRestClient)
                        {
                            await ((IWebServiceRestClient)channel).OpenChannelAsync();
                        }
                        else channel.OpenChannel();
                    }
                }

                // Aggiorno lo stato dei canali
                //NetworkState = (_channels != null && _channels.Count > 0) ?
                //  _channels.Min(c => c.ChannelState) : EChannelStates.Connecting;
                NetworkState = (_channels != null && _channels.Count > 0 && _channels.Min(c => c.ChannelState) != EChannelStates.Online) ? _channels.Min(c => c.ChannelState) :
                                (DateTime.Now.Subtract(_lastRecTime).TotalSeconds < _recTimeout ? EChannelStates.Online : EChannelStates.Offline);

                // Invio i messaggi in attesa gestendo gli opportuni Timeout
                var sendState = Send();

                // Scodo e memorizzo i messaggi ricevuti dai canali
                var receiveState = Receive();

                return sendState >= receiveState ? receiveState : sendState;
            }
            catch (Exception ex)
            {
                string log = DateTime.Now.ToString("HH:mm:ss.FFF") + ";" + ToString() + ";(MANAGE COMPONENT): " + ex.Message;
                _manager.LogsFileUpdate($"{Code} LOG_SOCKET.csv", new List<string> { log });

                return EComponentStates.Error;
            }
        }

        #endregion

        #region Protected Methods

        protected override Telegram ParseMessage(string message, TrafficChannel channel, out Telegram responseTelegram)
        {
            var telegram = new Movitrans_OUT_Tel();
            telegram.ParseReceivedMessage(message, out responseTelegram);
            return telegram;
        }

        #endregion

        #region Private Methods

        private DataRow INSERT_HAL_Agv(string agvCode, string Alarm, int AlrNum, DbConnection conn)
        {
            DataRow row = null;
            string error = null;

            try
            {
                var query = $@"
                            DECLARE @RC int
                            DECLARE @Agv nvarchar(50)
                            DECLARE @Alarm nvarchar(MAX)
                            DECLARE @Alr_num int
                            DECLARE @Error nvarchar(255)

                            set @Agv = {agvCode.SqlFormat()}
                            set @Alarm = {Alarm.SqlFormat()}
                            set @Alr_num = {AlrNum.SqlFormat()}

                        EXECUTE @RC = [dbo].[msp_INSERT_HAL_Agv] 
                                @Agv
                               ,@Alarm
                               ,@Alr_num
                               ,@Error OUTPUT

                    SELECT @RC          AS 'RetVal'
                          ,@Error       AS 'Error'";

                var dt = DbUtils.ExecuteDataTable(query, conn, true, out error);
                if (dt != null && dt.Rows.Count > 0 && string.IsNullOrWhiteSpace(error))
                    row = dt.Rows[0];
                else
                {
                    row = null;
                    if (error != null)
                    {
                        Global.Instance.Log(error, LogLevels.Fatal, "Movitrans_Ctr", "INSERT_HAL_Agv");
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Global.Instance.Log(ex.Message, LogLevels.Fatal, "Movitrans_Ctr", "INSERT_HAL_Agv");
            }

            return row;
        }

        private string GetMOV_Error(short ErrorCode)
        {
            string retVal = string.Empty;

            if (ErrorCode == 0) return retVal;

            retVal = $"({ErrorCode.ToString()})";

            switch (ErrorCode)
            {
                case (short)MOV_Error.Sovracorrente:
                    retVal += " " + "Sovracorrente";
                    break;

                case (short)MOV_Error.AnomaliaControlloUCE:
                    retVal += " " + "Anomalia Controllo UCE";
                    break;

                case (short)MOV_Error.MancanzaFaseRete:
                    retVal += " " + "Mancanza Fase Rete";
                    break;

                case (short)MOV_Error.TensioneCircuitoIntermedio:
                    retVal += " " + "Tensione Circuito Intermedio";
                    break;

                case (short)MOV_Error.Sovratemperatura:
                    retVal += " " + "Sovratemperatura";
                    break;

                case (short)MOV_Error.MorsettoEsterno:
                    retVal += " " + "Morsetto Esterno";
                    break;

                case (short)MOV_Error.TimeoutSBus2:
                    retVal += " " + "Timeout-SBus #2";
                    break;

                case (short)MOV_Error.TimeoutSBus1:
                    retVal += " " + "Timeout-SBus #1";
                    break;

                case (short)MOV_Error.Hardware:
                    retVal += " " + "Hardware";
                    break;

                case (short)MOV_Error.SincronizzazioneEsterna:
                    retVal += " " + "Sincronizzazione Esterna";
                    break;

                case (short)MOV_Error.SezionePotenza:
                    retVal += " " + "Sezione Potenza";
                    break;
            }

            return retVal;
        }

        private string GetMOV_Warning(short WarningCode)
        {
            string retVal = string.Empty;

            if (WarningCode == 0) return retVal;

            retVal = $"({WarningCode.ToString()})";

            switch (WarningCode)
            {
                case (short)MOV_Warning.Nessuna_Abilitazione:
                    retVal += " " + "Nessuna Abilitazione";
                    break;

                case (short)MOV_Warning.Abilitazione:
                    retVal += " " + "Abilitazione";
                    break;

                case (short)MOV_Warning.NonPronto:
                    retVal += " " + "Non Pronto";
                    break;
            }

            return retVal;
        }

        #endregion
    }
}
