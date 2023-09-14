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
    public class SimulaAgv_Cradle_Tel : SimulaTelBase
    {
        #region Members

        public class ShuttleCradleCommand
        {
            #region Members/Properties

            public EMachineCommands Command { get; set; }

            public EMachineLocationTypes LocationType { get; set; }

            public int CradleID { get; set; }

            public int UdcCount { get; set; }

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
        }

        #endregion

        #region Properties

        public int MachineID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public List<ShuttleCradleCommand> CradleCommands
        {
            get { return GetProperty<List<ShuttleCradleCommand>>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Constructor/Destructor

        public SimulaAgv_Cradle_Tel() : base() { }

        public SimulaAgv_Cradle_Tel(string wcsIdentity, string controllerIdentity)
            : base(wcsIdentity, controllerIdentity)
        {
            CradleCommands = new List<ShuttleCradleCommand>();
        }

        public SimulaAgv_Cradle_Tel(ETelegramTypes telegramType, string wcsIdentity, string controllerIdentity, int id = 0)
            : base(telegramType, wcsIdentity, controllerIdentity, id)
        {
            CradleCommands = new List<ShuttleCradleCommand>();
        }

        static SimulaAgv_Cradle_Tel()
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
            MachineID = int.Parse(body[0]);
            MissionID = long.Parse(body[1]);

            CradleCommands.Clear();
            /*
            for (int i = 2; i < body.Count; i += 13)
            {
                var cradle = new ShuttleCradleCommand();

                cradle.Command = (EMachineCommands)int.Parse(body[0 + i]);
                cradle.LocationType = (EMachineLocationTypes)Enum.Parse(typeof(EMachineLocationTypes), body[1 + i]);
                cradle.CradleID = int.Parse(body[2 + i]);
                cradle.UdcCount = int.Parse(body[3 + i]);
                cradle.RackNum = int.Parse(body[4 + i]);
                cradle.X = int.Parse(body[5 + i]);
                cradle.Y = int.Parse(body[6 + i]);
                cradle.Z = int.Parse(body[7 + i]);
                cradle.W = int.Parse(body[8 + i]);
                cradle.QuotaX = int.Parse(body[9 + i]);
                cradle.QuotaY = int.Parse(body[10 + i]);
                cradle.QuotaZ = int.Parse(body[11 + i]);
                cradle.QuotaW = int.Parse(body[12 + i]);

                CradleCommands.Add(cradle);
            }
            */
        }

        protected override string DONE()
        {
            List<string> data = new List<string>()
            {
                $"{MachineID}",
                $"{MissionID}"
            };

            foreach (ShuttleCradleCommand cradle in CradleCommands)
            {
                data.AddRange(new string[]{
                    $"{((int)cradle.CommandResult).ToString().PadLeft(2,'0')}",
                    $"{cradle.LocationType}",
                    $"{cradle.CradleID.ToString().PadLeft(2,'0')}",
                    $"{cradle.UdcCount.ToString().PadLeft(2,'0')}",
                    $"{"0".PadLeft(2,'0')}",
                    $"{cradle.RackNum.ToString().PadLeft(3,'0')}",
                    $"{cradle.X.ToString().PadLeft(3,'0')}",
                    $"{cradle.Y.ToString().PadLeft(3,'0')}",
                    $"{cradle.Z.ToString().PadLeft(3,'0')}",
                    $"{cradle.W.ToString().PadLeft(3,'0')}",
                    $"{cradle.QuotaX.ToString().PadLeft(6,'0')}",
                    $"{cradle.QuotaY.ToString().PadLeft(6,'0')}",
                    $"{cradle.QuotaZ.ToString().PadLeft(6,'0')}",
                    $"{cradle.QuotaW.ToString().PadLeft(6,'0')}",
                });
            }

            return string.Join(_separator, data.ToArray());
        }

        #endregion
    }
}
