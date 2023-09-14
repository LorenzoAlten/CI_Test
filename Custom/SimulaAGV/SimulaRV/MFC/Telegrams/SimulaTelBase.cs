using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwAgilogDll.MFC;
using mSwDllMFC;
using mSwDllUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulaRV
{
    public class SimulaTelBase : Telegram
    {
        #region Members

        protected string _separator = ";";

        #endregion

        #region Properties

        public ETelegramTypes TelegramType { get; set; }

        public bool SendAck
        {
            get
            {
                return TelegramType != ETelegramTypes.ACKT &&
                       TelegramType != ETelegramTypes.NACK;
            }
        }

        public string WcsIdentity
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string ControllerIdentity
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public int PingMillisec
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public string Position
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public long MissionID
        {
            get { return GetProperty<long>(); }
            set { SetProperty(value); }
        }

        public string UDC_Barcode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public int UDC_Type
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Cla_Lenght
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Cla_Width
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Cla_Height
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Weight
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public bool UDC_Barcode_Error
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public int ErrorCode
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int DifferentDestinationsNumber
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int UdcsNumber
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int UdcsMissing
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public string BayCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public List<BatchDestination> BatchDestinations
        {
            get { return GetProperty<List<BatchDestination>>(); }
            set { SetProperty(value); }
        }


        #endregion

        #region Constructor/Destructor

        public SimulaTelBase() { }

        public SimulaTelBase(string wcsIdentity, string controllerIdentity)
            : base()
        {
            WcsIdentity = wcsIdentity;
            ControllerIdentity = controllerIdentity;
            TelegramType = ETelegramTypes.UNKNOWN;
        }

        public SimulaTelBase(ETelegramTypes telegramType, string wcsIdentity, string controllerIdentity, int id = 0)
            : base()
        {
            TelegramType = telegramType;
            WcsIdentity = wcsIdentity;
            ControllerIdentity = controllerIdentity;
            ID = id;
        }

        static SimulaTelBase()
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

            // Ricavo i "pezzi" del messaggio
            var parts = message.Replace(STX, "").Replace(ETX, "").Split(_separator[0]);

            try
            {
                // [0] e [1] sono rispettivamente Sender e Receiver.
                // In questo caso il Sender è il Controller
                ControllerIdentity = parts[0];
                WcsIdentity = parts[1];

                // Il [2] è l'ID, da riutilizzare per ACK/NAK
                ID = int.Parse(parts[2]);

                // Il [3] è il tipo di telegramma
                TelegramType = (ETelegramTypes)Enum.Parse(typeof(ETelegramTypes), parts[3]);

                var body = parts.Skip(4).ToList();

                // Il contenuto dipende dal tipo di telegramma: l'interpretazione dei dati
                // è posizionale, pertanto a seconda del tipo avrò diverse proprietà
                switch (TelegramType)
                {
                    case ETelegramTypes.PING:
                        PING(body);
                        break;
                    case ETelegramTypes.ACKT:
                        ACKT();
                        break;
                    case ETelegramTypes.NACK:
                        NACK();
                        break;
                    case ETelegramTypes.DATA:
                        DATA(body);
                        break;
                    case ETelegramTypes.DEST:
                        DEST(body);
                        break;
                    case ETelegramTypes.LCDL:
                        LCDL(body);
                        break;
                    case ETelegramTypes.TKRD:
                        TKRD(body);
                        break;
                    case ETelegramTypes.TKUP:
                        TKUP(body);
                        break;
                    case ETelegramTypes.TKDL:
                        TKDL(body);
                        break;
                    case ETelegramTypes.ERRT:
                        ERRT(body);
                        break;
                    case ETelegramTypes.PKEX:
                        PKEX(body);
                        break;
                    case ETelegramTypes.MOVE:
                        MOVE(body);
                        break;
                    case ETelegramTypes.STSP:
                        STSP(body);
                        break;
                    case ETelegramTypes.BTST:
                        BTCH(body);
                        break;
                }

                // Se necessario, predispongo l'invio dell'ACK, riutilizzando
                // lo stesso ID del mittente
                if (SendAck)
                {
                    responseTelegram = new SimulaHdl_Tel(ETelegramTypes.ACKT, WcsIdentity, ControllerIdentity, ID);
                }

                // Se è un NAK dovrò ritentare l'invio messaggio
                RetrialRequested = TelegramType == ETelegramTypes.NACK;
            }
            catch (Exception ex)
            {
                // In caso di errore, loggo e restituisco un NAK se serve
                Logger.Log($"Handling_Tel - ParseReceivedMessage Error= {ex.Message}", LogLevels.Fatal);

                if (SendAck) responseTelegram = new SimulaHdl_Tel(ETelegramTypes.NACK, WcsIdentity, ControllerIdentity, ID);
            }
        }

        public override string GetMessage()
        {
            // Compongo l'Header del messaggio: per l'ID
            // messaggio, se ho lasciato il default (0), allora
            // inserisco il PlaceHolder in modo che sia il Controller
            // a battezzarlo, altrimenti se ho indicato un ID
            // esplicito (ACK, NAK), uso quello
            string message = STX
                + WcsIdentity
                + _separator
                + ControllerIdentity
                + _separator
                + (ID <= 0 ? TelegramIDPlaceHolder : ID.ToString().PadLeft(4, '0'))
                + _separator
                + TelegramType.ToString()
                + _separator;

            switch (TelegramType)
            {
                case ETelegramTypes.PING:
                    message += PING(new List<string> { PingMillisec.ToString() });
                    break;
                case ETelegramTypes.ACKT:
                    message += ACKT();
                    break;
                case ETelegramTypes.NACK:
                    message += NACK();
                    break;
                case ETelegramTypes.DREQ:
                    message += DREQ();
                    break;
                case ETelegramTypes.CTRL:
                    message += CTRL();
                    break;
                case ETelegramTypes.LCAP:
                    message += LCAP();
                    break;
                case ETelegramTypes.LPRE:
                    message += LPRE();
                    break;
                case ETelegramTypes.TKDT:
                    message += TKDT();
                    break;
                case ETelegramTypes.TKER:
                    message += TKER();
                    break;
                case ETelegramTypes.ERRT:
                    message += ERRT(new List<string> { ErrorCode.ToString() });
                    break;
                case ETelegramTypes.DONE:
                    message += DONE();
                    break;
                case ETelegramTypes.STSP:
                    message += STSP();
                    break;
                case ETelegramTypes.BTST:
                    message += BTST();
                    break;
            }

            message += ETX;

            return message;
        }

        public override string GetSignature()
        {
            if (TelegramType == ETelegramTypes.PING)
                return ETelegramTypes.PING.ToString();

            return GetMessage();
        }

        #endregion

        #region Protected Methods

        protected virtual string PING(List<string> body)
        {
            if (body.Count <= 0)
                return string.Empty;

            PingMillisec = int.Parse(body[0]);

            List<string> data = new List<string>
            {
                $"{PingMillisec}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected virtual string ACKT()
        {
            return string.Empty;
        }
        protected virtual string NACK()
        {
            return string.Empty;
        }
        protected virtual void DATA(List<string> body)
        {
        }
        protected virtual string DREQ()
        {
            return string.Empty;
        }
        protected virtual string CTRL()
        {
            List<string> data = new List<string>
            {
                $"{Position}",
                $"{MissionID}",
                $"{UDC_Barcode}",
                $"{UDC_Type}",
                $"{Cla_Lenght}",
                $"{Cla_Width}",
                $"{Cla_Height}",
                $"{Weight}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected virtual void DEST(List<string> body)
        {
        }
        protected virtual void LCDL(List<string> body)
        {
        }
        protected virtual string LCAP()
        {
            return string.Empty;
        }
        protected virtual string LPRE()
        {
            return string.Empty;
        }
        protected virtual void TKRD(List<string> body)
        {
        }
        protected virtual void TKDL(List<string> body)
        {
        }
        protected virtual string TKDT()
        {
            return string.Empty;
        }
        protected virtual string TKER()
        {
            return string.Empty;
        }
        protected virtual void TKUP(List<string> body)
        {
        }
        protected virtual string ERRT(List<string> body)
        {
            if (body.Count <= 0)
                return string.Empty;

            ErrorCode = int.Parse(body[0]);

            List<string> data = new List<string>
            {
                $"{ErrorCode}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected virtual void PKEX(List<string> body)
        {
        }
        protected virtual string DONE()
        {
            return string.Empty;
        }
        protected virtual void MOVE(List<string> body)
        {
        }

        protected virtual string STSP()
        {
            return string.Empty;
        }

        protected virtual void STSP(List<string> body)
        {

        }

        protected virtual string RAZETEL()
        {
            return string.Empty;
        }

        protected virtual void RAZETEL(List<string> body)
        {

        }

        protected virtual string BTST()
        {
            return string.Empty;
        }
        
        protected virtual void BTCH(List<string> body)
        {
        }
        #endregion
    }
}
