using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllMFC;
using mSwDllUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulaRV
{
    public class SimulaShuttle_Tel : SimulaTelBase
    {
        #region Members

        public class ShuttleCradleCommand
        {
            #region Members/Properties

            public EMachineCommands Command { get; set; }

            public EMachineLocationTypes LocationType { get; set; }

            public int CradleID { get; set; }

            public int UdcCount { get; set; }

            public int CradleCapacity { get; set; }

            public int RackNum { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Z { get; set; }

            public int W { get; set; }

            public int QuotaX { get; set; }

            public int QuotaY { get; set; }

            public int QuotaZ { get; set; }

            public int QuotaW { get; set; }

            public EMachineCommand_Results CommandResult { get; set; }

            public List<ShuttleUdcData> UdcDatas { get; set; }

            #endregion

            #region Override

            public override string ToString()
            {
                return $"{CradleID} - {Command} - {RackNum} - {X}|{Y}|{Z}|{W}";
            }

            public override bool Equals(object obj)
            {
                return obj is ShuttleCradleCommand &&
                       ((ShuttleCradleCommand)obj).CradleID == CradleID;
            }

            public override int GetHashCode()
            {
                return CradleID;
            }

            #endregion

            public ShuttleCradleCommand()
            {
                UdcDatas = new List<ShuttleUdcData>();
            }
        }

        public class ShuttleUdcData
        {
            #region Members/Properties

            public long MissionID { get; set; }

            public string UdcBarcode { get; set; }

            public int UdcType { get; set; }

            #endregion

            #region Override

            public override string ToString()
            {
                return $"{MissionID} - {UdcBarcode} - {UdcType}";
            }

            public override bool Equals(object obj)
            {
                return obj is ShuttleUdcData &&
                       ((ShuttleUdcData)obj).UdcBarcode == UdcBarcode;
            }

            public override int GetHashCode()
            {
                return UdcType;
            }

            #endregion
        }

        #endregion

        #region Properties

        public string MachineID
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public List<ShuttleCradleCommand> CradleCommands
        {
            get { return GetProperty<List<ShuttleCradleCommand>>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Constructor/Destructor

        public SimulaShuttle_Tel() : base()
        {
            CradleCommands = new List<ShuttleCradleCommand>();
        }

        public SimulaShuttle_Tel(string wcsIdentity, string controllerIdentity)
            : base(wcsIdentity, controllerIdentity)
        {
            CradleCommands = new List<ShuttleCradleCommand>();
        }

        public SimulaShuttle_Tel(ETelegramTypes telegramType, string wcsIdentity, string controllerIdentity, int id = 0)
            : base(telegramType, wcsIdentity, controllerIdentity, id)
        {
            CradleCommands = new List<ShuttleCradleCommand>();
        }

        static SimulaShuttle_Tel()
        {
            STX = ((char)0x02).ToString();
            ETX = ((char)0x03).ToString();
            FixedLength = 0;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Protected Methods

        protected override void MOVE(List<string> body)
        {
            MachineID = body[0];
            MissionID = long.Parse(body[1]);

            CradleCommands.Clear();

            for (int i = 2; i < body.Count; )
            {
                var cradle = new ShuttleCradleCommand();

                cradle.Command = (EMachineCommands)int.Parse(body[0 + i]);
                cradle.LocationType = (EMachineLocationTypes)Enum.Parse(typeof(EMachineLocationTypes), body[1 + i]);
                cradle.CradleID = int.Parse(body[2 + i]);
                cradle.UdcCount = int.Parse(body[3 + i]);
                cradle.CradleCapacity = int.Parse(body[4 + i]);
                cradle.RackNum = int.Parse(body[5 + i]);
                cradle.X = int.Parse(body[6 + i]);
                cradle.Y = int.Parse(body[7 + i]);
                cradle.Z = int.Parse(body[8 + i]);
                cradle.W = int.Parse(body[9 + i]);
                cradle.QuotaX = int.Parse(body[10 + i]);
                cradle.QuotaY = int.Parse(body[11 + i]);
                cradle.QuotaZ = int.Parse(body[12 + i]);
                cradle.QuotaW = int.Parse(body[13 + i]);

                for (int j = i; j < (i + cradle.CradleCapacity * 3); j += 3)
                {
                    ShuttleUdcData udc = new ShuttleUdcData();
                    udc.MissionID = long.Parse(body[14 + j]);
                    udc.UdcBarcode = body[15 + j];
                    udc.UdcType = int.Parse(body[16 + j]);

                    cradle.UdcDatas.Add(udc);
                }

                CradleCommands.Add(cradle);

                i += 14 + cradle.CradleCapacity * 3;
            }
        }

        protected override string DONE()
        {
            List<string> data = new List<string>()
            {
                $"{MachineID}",
                $"{MissionID.ToString().PadLeft(9, '0')}"
            };

            foreach (ShuttleCradleCommand cradle in CradleCommands)
            {
                data.AddRange(new string[]{
                    $"{((int)cradle.CommandResult).ToString().PadLeft(2,'0')}",
                    $"{cradle.LocationType}",
                    $"{cradle.CradleID.ToString().PadLeft(2,'0')}",
                    $"{cradle.UdcCount.ToString().PadLeft(2,'0')}",
                    $"{cradle.CradleCapacity.ToString().PadLeft(2,'0')}",
                    $"{cradle.RackNum.ToString().PadLeft(3,'0')}",
                    $"{cradle.X.ToString().PadLeft(3,'0')}",
                    $"{cradle.Y.ToString().PadLeft(3,'0')}",
                    $"{cradle.Z.ToString().PadLeft(3,'0')}",
                    $"{cradle.W.ToString().PadLeft(3,'0')}",
                    $"{cradle.QuotaX.ToString().PadLeft(5,'0')}",
                    $"{cradle.QuotaY.ToString().PadLeft(5,'0')}",
                    $"{cradle.QuotaZ.ToString().PadLeft(5,'0')}",
                    $"{cradle.QuotaW.ToString().PadLeft(5,'0')}",
                });

                foreach (ShuttleUdcData udc in cradle.UdcDatas)
                {
                    data.AddRange(new string[]{
                    $"{udc.MissionID.ToString().PadLeft(9, '0')}",
                    $"{(udc.UdcBarcode??string.Empty).PadLeft(10, '0')}",
                    $"{udc.UdcType.ToString().PadLeft(2, '0')}" });
                }
            }

            return string.Join(_separator, data.ToArray());
        }

        #endregion
    }
}
