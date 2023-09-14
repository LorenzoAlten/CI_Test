using mSwDllMFC;
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
using mSwAgilogDll.Errevi;
using mSwDllComms;
using mSwDllWcfClient;

namespace SimulaRV
{
    public class SimulaShuttle_Ctr : TrafficController
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

        public SimulaShuttle_Ctr(SqlConnection connection, SimulaShuttle_Mgr handling, DataRow row)
            : base(connection, handling, row)
        {
            Telegram.InitStaticVariables<SimulaShuttle_Tel>();

            Telegram_STX = SimulaShuttle_Tel.STX;
            Telegram_ETX = SimulaShuttle_Tel.ETX;
            Telegram_FixedLength = SimulaShuttle_Tel.FixedLength;

            _lastSendTime = DateTime.MinValue;
            _lastRecTime = DateTime.MinValue;
            _delayTime = 0;
        }

        #endregion

        #region Public Metohds

        public override Telegram GetTestTelegram(out bool waitForResponse)
        {
            SimulaShuttle_Tel telegram = new SimulaShuttle_Tel(ETelegramTypes.PING, WcsIdentity, Code);
            waitForResponse = true;

            return telegram;
        }

        public async Task<string> WriteVariableShuttleAsync(ESimulaShuttleCtr_Variables variable, int value)
        {
            if (_cfgCradles == null)
            {
                return "Il controller non è configurato per eseguire scrittura di variabili";
            }

            var cradleLift = _cfgCradles.FirstOrDefault(c => c.MOD_PLN_Code.StartsWith("LIFT"));
            int startAddressLift = cradleLift != null ? cradleLift.MOD_PLC_StatusAddress.Value : -1;
            int scbLift = cradleLift != null ? cradleLift.MOD_PLC_SCB_Id.Value : -1;
            string areaLift = cradleLift != null ? cradleLift.MOD_PLC_Area : "";

            var cradleShuttle = _cfgCradles.FirstOrDefault(c => c.MOD_PLN_Code.StartsWith("NAV") || c.MOD_PLN_Code.StartsWith("TR"));
            int startAddressShuttle = cradleShuttle.MOD_PLC_StatusAddress.Value;
            int scbShuttle = cradleShuttle.MOD_PLC_SCB_Id.Value;
            string areaShuttle = cradleShuttle.MOD_PLC_Area;

            string command = null;
            int startAddress = -1;
            int scb = 0;
            string area = null;

            switch (variable)
            {
                case ESimulaShuttleCtr_Variables.Stato_Mix_Lift:
                    startAddress = startAddressLift;
                    command = Utils.Int16ToPlcString((short)value);
                    scb = scbLift;
                    area = areaLift;
                    break;

                case ESimulaShuttleCtr_Variables.QuotaY:
                    startAddress = startAddressLift + 1;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbLift;
                    area = areaLift;
                    break;

                case ESimulaShuttleCtr_Variables.Debordo_Culla:
                    startAddress = startAddressLift + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbLift, areaLift, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 0, (value > 0), _swapBYTEForBool);
                    scb = scbLift;
                    area = areaLift;
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Shuttle:
                    startAddress = startAddressLift + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbLift, areaLift, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 1, (value > 0), _swapBYTEForBool);
                    scb = scbLift;
                    area = areaLift;
                    break;

                case ESimulaShuttleCtr_Variables.Culla_Al_Piano:
                    startAddress = startAddressLift + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbLift, areaLift, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 2, (value > 0), _swapBYTEForBool);
                    scb = scbLift;
                    area = areaLift;
                    break;

                case ESimulaShuttleCtr_Variables.Stato_Mix_Shuttle:
                    startAddress = startAddressShuttle;
                    command = Utils.Int16ToPlcString((short)value);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.QuotaX:
                    startAddress = startAddressShuttle + 1;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Satellite:
                    startAddress = startAddressShuttle + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbShuttle, areaShuttle, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 0, (value > 0), _swapBYTEForBool);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc:
                    startAddress = startAddressShuttle + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbShuttle, areaShuttle, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 1, (value > 0), _swapBYTEForBool);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc_SX:
                    startAddress = startAddressShuttle + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbShuttle, areaShuttle, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 2, (value > 0), _swapBYTEForBool);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.Presenza_Udc_DX:
                    startAddress = startAddressShuttle + 3;
                    command = await Global.Instance.ReadDataReservedAsync(scbShuttle, areaShuttle, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 3, (value > 0), _swapBYTEForBool);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                case ESimulaShuttleCtr_Variables.QuotaZ:
                    startAddress = startAddressShuttle + 26;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbShuttle;
                    area = areaShuttle;
                    break;

                default:
                    break;
            }

            if (startAddress < 0 || string.IsNullOrWhiteSpace(command))
                return "Variabile non riconosciuta";

            return await Global.Instance.WriteDataReservedAsync(scb, area, startAddress, command);
        }

        public async Task<string> WriteVariableTrasloAsync(ERVTrasloCtr_Variables variable, int value, string plncontroller)
        {
            if (_cfgCradles == null)
            {
                return "Il controller non è configurato per eseguire scrittura di variabili";
            }

            var cradleTraslo = _cfgCradles.FirstOrDefault(c => c.MOD_PLN_Code == plncontroller);
            int startAddressTraslo = cradleTraslo.MOD_PLC_StatusAddress.Value;
            int scbTraslo = cradleTraslo.MOD_PLC_SCB_Id.Value;
            string areaTraslo = cradleTraslo.MOD_PLC_Area;

            string command = null;
            int startAddress = -1;
            int scb = 0;
            string area = null;

            switch (variable)
            {
                case ERVTrasloCtr_Variables.Stato_Mix_Traslo:
                    startAddress = startAddressTraslo;
                    command = Utils.Int16ToPlcString((short)value);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.QuotaX:
                    startAddress = startAddressTraslo + 1;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.QuotaY:
                     startAddress = startAddressTraslo + 3;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.QuotaZ:
                    startAddress = startAddressTraslo + 5;
                    command = Utils.Int32ToPlcString(value, _swapDWORD);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.Centro_Forche:
                    startAddress = startAddressTraslo + 7;
                    command = await Global.Instance.ReadDataReservedAsync(scbTraslo, areaTraslo, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 0, (value > 0), _swapBYTEForBool);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.Presenza_Udc:
                    startAddress = startAddressTraslo + 7;
                    command = await Global.Instance.ReadDataReservedAsync(scbTraslo, areaTraslo, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 1, (value > 0), _swapBYTEForBool);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.Presenza_Udc_SX:
                    startAddress = startAddressTraslo + 7;
                    command = await Global.Instance.ReadDataReservedAsync(scbTraslo, areaTraslo, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 2, (value > 0), _swapBYTEForBool);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                case ERVTrasloCtr_Variables.Presenza_Udc_DX:
                    startAddress = startAddressTraslo + 7;
                    command = await Global.Instance.ReadDataReservedAsync(scbTraslo, areaTraslo, startAddress, 1);
                    command = Utils.BoolToPlcString(command, 3, (value > 0), _swapBYTEForBool);
                    scb = scbTraslo;
                    area = areaTraslo;
                    break;

                default:
                    break;
            }

            if (startAddress < 0 || string.IsNullOrWhiteSpace(command))
                return "Variabile non riconosciuta";

            return await Global.Instance.WriteDataReservedAsync(scb, area, startAddress, command);
        }

        #endregion

        #region Protected Methods

        protected override bool Init()
        {
            if (!base.Init()) return false;

            try
            {
                // Recupero le culle contenenti la configurazione
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
            var telegram = new SimulaShuttle_Tel(WcsIdentity, Code);
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
            if (((SimulaShuttle_Tel)telegram).TelegramType == ETelegramTypes.ACKT)
                _lastRecTime = DateTime.Now;

            base.OnMsgReceived(sender, telegram);
        }

        #endregion
    }
    
    public enum ESimulaShuttleCtr_Variables
    {
        QuotaY,
        QuotaX,
        QuotaZ,
        Culla_Al_Piano,
        Presenza_Shuttle,
        Presenza_Udc,
        Presenza_Satellite,
        Stato_Mix_Lift,
        Stato_Mix_Shuttle,
        Debordo_Culla,
        Presenza_Udc_SX,
        Presenza_Udc_DX,
    }

    public enum ERVTrasloCtr_Variables
    {
        QuotaY,
        QuotaX,
        QuotaZ,
        Centro_Forche,
        Stato_Mix_Traslo,
        Presenza_Udc,
        Presenza_Udc_SX,
        Presenza_Udc_DX,
    }
}
