using mSwAgilogDll;
using mSwAgilogDll.SEW;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMgr.AppData
{
    public class Common
    {
        #region Singleton
        private static readonly Common instance = new Common();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Common()
        {
        }

        private Common()
        {
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(_conn));
        }

        public static Common Instance
        {
            get { return instance; }
        }
        #endregion

        private SqlConnection _conn;
        private AgilogDll.RunUtils _utils;

        public List<MisMissionAgv> MissionsList { get; private set; } = new List<MisMissionAgv>();
        public List<SEW_AGV> Agvs { get; private set; } = new List<SEW_AGV>();

        internal void LoadMissions()
        {
            var missions = _utils.GetMissionsAGV(_conn, true, false);

            lock (MissionsList)
            {
                MissionsList = missions ?? new List<MisMissionAgv>();
            }
        }

        internal void LoadAgvs()
        {
            try
            {
                var agvs = AgvMachine.GetList(_conn);

                foreach (SEW_AGV agv in agvs.Where(x => x.AGV_CTR_Id > 0 && x.CTR_Enabled))
                {
                    Agvs.Add(agv);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevels.Fatal);
                return;
            }
        }
    }
}
