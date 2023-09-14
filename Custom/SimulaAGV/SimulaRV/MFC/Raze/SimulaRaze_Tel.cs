using AgilogDll.EntitiesDepallettizer;
using mSwAgilogDll.Errevi;
using mSwDllMFC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulaRV
{
    public class SimulaRaze_Tel : SimulaTelBase
    {
        #region Members

        #endregion

        #region Properties

        public string TelLength { get; set; }

        public string Operation { get; set; }

        public new ERazeTelegramTypes TelegramType { get; set; }

        #endregion

        #region Constructor/Destructor

        public SimulaRaze_Tel() : base() { }

        public SimulaRaze_Tel(string wcsIdentity, string controllerIdentity)
            : base(wcsIdentity, controllerIdentity)
        {
        }

        public SimulaRaze_Tel(ETelegramTypes telegramType, string wcsIdentity, string controllerIdentity, int id = 0)
            : base(telegramType, wcsIdentity, controllerIdentity, id)
        {
        }

        static SimulaRaze_Tel()
        {
            STX = ((char)0x02).ToString();
            ETX = ((char)0x03).ToString();
            FixedLength = 0;
        }

        #endregion

        #region Public Methods

        public override void ParseReceivedMessage(string message, out Telegram responseTelegram)
        {
            responseTelegram = null;

            List<string> messageSplitted = message.Split('#').ToList();

            TelLength = messageSplitted[0];

            Operation = messageSplitted[1];

            TelegramType = (ERazeTelegramTypes)Enum.Parse(typeof(ERazeTelegramTypes), messageSplitted[2]);
        }

        #endregion
    }
}
