using mSwAgilogDll;
using mSwAgilogDll.SEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AgvMgr.AppData
{
    public class CustomizedModelVisual3D : ModelVisual3D
    {
        public bool IsTransponder = false;
        public bool IsAgv = false;
        public bool IsSfondo = false;
        public bool IsConveyor = false;
        public bool IsPath = false;
        public int X;
        public int Y;
        public double Angle;
        public long? Mission = null;
        public State_Flags AgvState = State_Flags.Start;
        public AGV_Enabling_Mode EnablingMode = AGV_Enabling_Mode.None;
        public bool Load;
        public string AgvCode { get; set; }
        public string ConveyorCode { get; set; }
        public Agv_Station_Status ConveyorStatus { get; set; }
        public string Destination { get; set; }
        public string Udc { get; set; }

        public CustomizedModelVisual3D() : base()
        {
        }
    }
}
