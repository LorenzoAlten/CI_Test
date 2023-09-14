using mSwDllMFC;
using System;
using System.Data;
using System.Data.SqlClient;
using mSwAgilogDll.Errevi;

namespace SimulaRV
{
    public class SimulaHdl_Ctr : TrafficController
    {
        #region Members

        protected DateTime _lastSendTime;
        protected DateTime _lastRecTime;
        protected double _delayTime;

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

        public SimulaHdl_Ctr(SqlConnection connection, SimulaHdl_Mgr handling, DataRow row)
            : base(connection, handling, row)
        {
            Telegram.InitStaticVariables<SimulaHdl_Tel>();

            Telegram_STX = SimulaHdl_Tel.STX;
            Telegram_ETX = SimulaHdl_Tel.ETX;
            Telegram_FixedLength = SimulaHdl_Tel.FixedLength;

            _lastSendTime = DateTime.MinValue;
            _lastRecTime = DateTime.MinValue;
            _delayTime = 0;
        }

        #endregion

        #region Public Metohds

        public override Telegram GetTestTelegram(out bool waitForResponse)
        {
            SimulaHdl_Tel telegram = new SimulaHdl_Tel(ETelegramTypes.PING, WcsIdentity, Code);
            waitForResponse = true;

            return telegram;
        }

        #endregion

        #region Protected Methods

        protected override void SetPublicVariablesReadings()
        {
            // Inizializzo le variabili
            foreach (string name in Enum.GetNames(typeof(ESimulaHdlCtr_Variables)))
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
            var telegram = new SimulaHdl_Tel(WcsIdentity, Code);
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
            if (((SimulaHdl_Tel)telegram).TelegramType == ETelegramTypes.ACKT)
                _lastRecTime = DateTime.Now;

            base.OnMsgReceived(sender, telegram);
        }

        #endregion
    }

    public enum ESimulaHdlCtr_Variables
    {

    }
}
