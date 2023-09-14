using AgilogDll.EntitiesDepallettizer;
using mSwAgilogDll.Errevi;
using mSwAgilogDll.MFC;
using System.Collections.Generic;

namespace SimulaRV
{
    public class SimulaHdl_Tel : SimulaTelBase
    {
        #region Members

        #endregion

        #region Properties

        public string Destination
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public ETrackingErrors TrackingErrorCode
        {
            get { return GetProperty<ETrackingErrors>(); }
            set { SetProperty(value); }
        }

        public int BoxesStoraged
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public EstspStatus Status
        {
            get { return GetProperty<EstspStatus>(); }
            set { SetProperty(value); }
        }

        public bool CheckData
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string ItemCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool Wrapped
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public int Layers
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Packages
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public bool BasePallet
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool LastPallet
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Constructor/Destructor

        public SimulaHdl_Tel() : base() { }

        public SimulaHdl_Tel(string wcsIdentity, string controllerIdentity)
            : base(wcsIdentity, controllerIdentity)
        {
        }

        public SimulaHdl_Tel(ETelegramTypes telegramType, string wcsIdentity, string controllerIdentity, int id = 0)
            : base(telegramType, wcsIdentity, controllerIdentity, id)
        {
        }

        static SimulaHdl_Tel()
        {
            STX = ((char)0x02).ToString();
            ETX = ((char)0x03).ToString();
            FixedLength = 0;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Protected Methods

        protected override void DATA(List<string> body)
        {
            Position = body[0];
            MissionID = long.Parse(body[1]);
            UDC_Barcode = body[2];
            UDC_Type = int.Parse(body[3]);
            Destination = body[4];
        }
        protected override string DREQ()
        {
            List<string> data = new List<string>
            {
                $"{Position}",
                $"{UDC_Barcode}",
                $"{(CheckData ? 1 : 0)}",
                $"{UDC_Type}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected override void DEST(List<string> body)
        {
            MissionID = long.Parse(body[0]);
            Destination = body[1];
        }
        protected override void LCDL(List<string> body)
        {
            MissionID = long.Parse(body[0]);
        }
        protected override string LCAP()
        {
            List<string> data = new List<string>
            {
                $"{Position}",
                $"{MissionID}",
                $"{UDC_Barcode}",
                $"{UDC_Type}",
                $"{Destination}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected override string LPRE()
        {
            List<string> data = new List<string>
            {
                $"{Position}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected override void TKRD(List<string> body)
        {
            Position = body[0];
        }
        protected override void TKDL(List<string> body)
        {
            Position = body[0];
        }
        protected override string TKDT()
        {
            return LCAP();
        }
        protected override string TKER()
        {
            List<string> data = new List<string>
            {
                $"{TrackingErrorCode}",
            };

            return string.Join(_separator, data.ToArray());
        }
        protected override void TKUP(List<string> body)
        {
            DATA(body);
        }
        protected override void PKEX(List<string> body)
        {
            Position = body[0];
            MissionID = long.Parse(body[1]);
            ItemCode = body[2];
            Wrapped = int.Parse(body[3]) > 0;
            Layers = int.Parse(body[4]);
            Packages = int.Parse(body[5]);
            BasePallet = int.Parse(body[6]) > 0;
            LastPallet = int.Parse(body[7]) > 0;
        }

        protected override string STSP()
        {
            List<string> data = new List<string>
            {
                $"{Position}",
                $"{Status}",
                $"{BoxesStoraged}"
                };

            return string.Join(_separator, data.ToArray());
        }

        protected override string BTST()
        {
            List<string> data = new List<string>
            {
                $"{BayCode}",
                $"{Position}",
                $"{UDC_Barcode}",
                $"{UdcsMissing}",
                $"{DifferentDestinationsNumber}"
            };

            if (BatchDestinations != null && BatchDestinations.Count > 0)
            {
                foreach (BatchDestination batchDestination in BatchDestinations)
                {
                    data.AddRange(new string[]{
                    $"{batchDestination.Destination}",
                    $"{batchDestination.UdcsNumber}"
                });
                }
            }

            return string.Join(_separator, data.ToArray());
        }

        protected override void BTCH(List<string> body)
        {
            Position = body[0];
            MissionID = long.Parse(body[1]);
            UDC_Barcode = body[2];
            UdcsNumber = int.Parse(body[3]);
            DifferentDestinationsNumber = int.Parse(body[4]);

            if (body.Count > 4)
            {
                for (int i = 5; i < body.Count; i += 2)
                {
                    BatchDestination batchDestination = new BatchDestination
                    {
                        Destination = body[i],
                        UdcsNumber = int.Parse(body[i + 1]),
                        Priority = 0
                    };

                    BatchDestinations.Add(batchDestination);
                }
            }
        }

        #endregion
    }
}
