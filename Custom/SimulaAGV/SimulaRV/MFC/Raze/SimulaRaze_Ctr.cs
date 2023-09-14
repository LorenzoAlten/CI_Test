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
using AgilogDll.MFC.Telegrams;

namespace SimulaRV
{
    public class SimulaRaze_Ctr : TrafficController
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

        public SimulaRaze_Ctr(SqlConnection connection, SimulaRaze_Mgr handling, DataRow row)
            : base(connection, handling, row)
        {
            _lastSendTime = DateTime.MinValue;
            _lastRecTime = DateTime.MinValue;
            _delayTime = 0;
        }

        #endregion

        #region Public Metohds

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
            var telegram = new SimulaRaze_Tel();
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
            base.OnMsgSent(sender, message);
        }

        protected override void OnMsgReceived(TrafficChannel sender, Telegram telegram)
        {
            base.OnMsgReceived(sender, telegram);
        }

        #endregion
    }

    public enum ESimulaRazeCtr_Variables
    {

    }
}
