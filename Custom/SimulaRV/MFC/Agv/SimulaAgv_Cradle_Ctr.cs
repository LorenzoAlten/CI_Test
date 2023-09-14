using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SimulaRV
{
    public class SimulaAgv_Cradle_Ctr : TrafficController
    {
        #region Members

        protected DateTime _lastSendTime;
        protected DateTime _lastRecTime;
        protected double _delayTime;

        protected List<HndModule> _cfgCradles;

        protected bool _swapDWORD;
        protected bool _swapBYTEForBool;

        #endregion

        #region Properties

        public DateTime LastSendTelegramTime;
        public string WcsIdentity { get; set; }

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

        public SimulaAgv_Cradle_Ctr(SqlConnection connection, SimulaAgv_Cradle_Mgr handling, DataRow row)
            : base(connection, handling, row)
        {
            Telegram.InitStaticVariables<SimulaAgv_Cradle_Tel>();

            Telegram_STX = SimulaAgv_Cradle_Tel.STX;
            Telegram_ETX = SimulaAgv_Cradle_Tel.ETX;
            Telegram_FixedLength = SimulaAgv_Cradle_Tel.FixedLength;

            _lastSendTime = DateTime.MinValue;
            _lastRecTime = DateTime.MinValue;
            _delayTime = 0;

            LastSendTelegramTime = DateTime.MinValue;
        }

        #endregion

        #region Public Metohds

        public override Telegram GetTestTelegram(out bool waitForResponse)
        {
            SimulaAgv_Cradle_Tel telegram = new SimulaAgv_Cradle_Tel(ETelegramTypes.PING, WcsIdentity, Code);
            waitForResponse = true;

            return telegram;
        }

        public async Task<string> WriteVariableAsync(ESimulaShuttleCtr_Variables variable, int value)
        {
            if (_cfgCradles == null)
            {
                return "Il controller non è configurato per eseguire scrittura di variabili";
            }

            int startAddressLift = _cfgCradles.FirstOrDefault(c => c.MOD_Code.StartsWith("LIFT")).MOD_PLC_StatusAddress.Value;
            int startAddressShuttle = _cfgCradles.FirstOrDefault(c => c.MOD_Code.StartsWith("RVS")).MOD_PLC_StatusAddress.Value;
            int startAddress = -1;
            string command = null;
            int scb = _cfgCradles[0].MOD_PLC_SCB_Id.Value;
            string area = _cfgCradles[0].MOD_PLC_Area;

            switch (variable)
            {
                case ESimulaShuttleCtr_Variables.QuotaY:
                    startAddress = startAddressLift;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    break;

                case ESimulaShuttleCtr_Variables.Debordo_Culla:
                    startAddress = startAddressLift + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 0, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Shuttle:
                    startAddress = startAddressLift + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 1, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.Culla_Al_Piano:
                    startAddress = startAddressLift + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 2, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.QuotaX:
                    startAddress = startAddressShuttle;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Satellite:
                    startAddress = startAddressShuttle + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 0, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc:
                    startAddress = startAddressShuttle + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 1, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc_SX:
                    startAddress = startAddressShuttle + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 2, (value > 0), _swapBYTEForBool);
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc_DX:
                    startAddress = startAddressShuttle + 2;
                    command = await Global.Instance.ReadDataAsync(scb, area, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 3, (value > 0), _swapBYTEForBool);
                    break;

                default:
                    break;
            }

            if (startAddress < 0 || string.IsNullOrWhiteSpace(command))
                return "Variabile non riconosciuta";

            return await Global.Instance.WriteDataAsync(scb, area, startAddress, command);
        }

        #endregion

        #region Protected Methods

        protected override bool Init()
        {
            if (!base.Init()) return false;

            try
            {
                // Recupero la culla del Lift contenente la configurazione
                // per le letture dirette da DB PLC per i parametri Real-Time da gestire
                var query = $@"
                    SELECT [MOD_Code]
                    FROM [HND_MODULES]
                    WHERE [MOD_HMT_Code] = 'CRD'
                    AND ISNULL([MOD_PLC_SCB_Id], -1) > 0
                    AND MOD_PLN_Code IN (SELECT STC_PLN_Code FROM STC_MACHINES)";

                var dt = DbUtils.ExecuteDataTable(query, _conn);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return true;
                }

                _cfgCradles = new List<HndModule>();
                foreach (DataRow row in dt.Rows)
                {
                    var cradle = new HndModule((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));
                    if (!cradle.GetByKey(row.GetValue(0)))
                        throw new Exception("Bad controller configuration. Check HND_MODULES table");

                    _cfgCradles.Add(cradle);
                }

                // Recupero il flag di Swap/No Swap dal protocollo
                var localConn = DbUtils.CloneConnection(Global.Instance.ConnLocal);
                query = $@"
                    SELECT [PRT_SwapDWORD], [PRT_SwapBYTEForBool]
                    FROM [SCB]
                    INNER JOIN [PRT] ON [PRT_Code] = [SCB_PRT_Code]
                    WHERE [SCB_Id] = {_cfgCradles[0].MOD_PLC_SCB_Id.Value.SqlFormat()}";
                dt = DbUtils.ExecuteDataTable(query, localConn);

                _swapDWORD = dt.Rows[0].GetValueB("PRT_SwapDWORD");
                _swapBYTEForBool = dt.Rows[0].GetValueB("PRT_SwapBYTEForBool");

                return true;
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
                throw ex;
            }
        }

        protected override void SetPublicVariablesReadings()
        {
            // Inizializzo le variabili
            foreach (string name in Enum.GetNames(typeof(ESimulaShuttleCtr_Variables)))
            {
                SetProperty(name, null);
            }
        }

        protected override string[] RefreshVariables(Telegram telegram)
        {
            return null;
        }

        protected override Telegram ParseMessage(string message, TrafficChannel channel, out Telegram responseTelegram)
        {
            var telegram = new SimulaAgv_Cradle_Tel(WcsIdentity, Code);
            telegram.ParseReceivedMessage(message, out responseTelegram);
            return telegram;
        }

        protected override string SetMessageID(string message, out int telegramID)
        {
            telegramID = 0;

            if (!message.Contains(Telegram.TelegramIDPlaceHolder))
                return message;

            var msgToSend = message.Replace(Telegram.TelegramIDPlaceHolder, _telegramID.ToString().PadLeft(4, '0'));
            _messages.Add(_telegramID, msgToSend);
            telegramID = _telegramID;

            _telegramID = _telegramID >= 9999 ? 1 : _telegramID + 1;

            return msgToSend;
        }

        protected override void OnMsgSent(TrafficChannel sender, string message)
        {
            // Memorizzo l'orario di invio ultimo messaggio se non è un ACKT
            if (!message.Contains($";{ETelegramTypes.ACKT}"))
                _lastSendTime = DateTime.Now;

            base.OnMsgSent(sender, message);
        }

        protected override void OnMsgReceived(TrafficChannel sender, Telegram telegram)
        {
            // Memorizzo l'orario di ultima ricezione messaggio se è un ACKT
            if (((SimulaAgv_Cradle_Tel)telegram).TelegramType == ETelegramTypes.ACKT)
                _lastRecTime = DateTime.Now;

            base.OnMsgReceived(sender, telegram);
        }

        #endregion
    }
}
