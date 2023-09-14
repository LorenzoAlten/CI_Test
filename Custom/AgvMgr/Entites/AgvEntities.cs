using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMgr.Entites
{
    public class AgvEntities
    {
        public string AGV_Code { get; set; }
        public string Old_AGV_Code { get; set; }
        public int CTR_ID { get; set; }
        public string CTR_Code { get; set; }
        public string CTR_MMG_Code { get; set; }
        public int CHL_Id { get; set; }
        public string CHL_IP { get; set; }
        public string CHL_Class { get; set; }
        public int CHL_Port { get; set; }
        public string CTR_Class { get; set; }
        public int? CTR_ID_Cradle { get; set; }

        public List<AgvCradleEntities> CradleEntities { get; set; } = new List<AgvCradleEntities>();

        public List<AgvEntities> GetList()
        {
            List<AgvEntities> agvEntities = new List<AgvEntities>();

            string query = $"SELECT AGV_Code, AGV_CTR_Id, CTR_Code, MOD_CTR_Id, CTR_Class, CHL_Id, CHL_IP, CHL_Port, CHL_Class, CTR_MMG_Code " +
                            "FROM MFC_CONTROLLERS " +
                            "JOIN MFC_CHANNELS " +
                            "ON CHL_CTR_Id = CTR_Id " +
                            "JOIN AGV_MACHINES " +
                            "ON AGV_CTR_Id = CTR_Id " +
                            "LEFT JOIN HND_MODULES " +
                            "ON MOD_PLN_Code = AGV_Code " +
                            "WHERE CTR_Enabled = 1 " +
                            "AND CHL_Enabled = 1";

            var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);

            if (dt.Rows.Count > 0)
            {
                agvEntities = dt.AsEnumerable().Select(x => new AgvEntities()
                {
                    AGV_Code = x.GetValue("AGV_Code"),
                    Old_AGV_Code = x.GetValue("AGV_Code"),
                    CTR_ID = x.GetValueI("AGV_CTR_Id"),
                    CTR_Code = x.GetValue("CTR_Code"),
                    CTR_MMG_Code = x.GetValue("CTR_MMG_Code"),
                    CHL_Id = x.GetValueI("CHL_Id"),
                    CHL_IP = x.GetValue("CHL_IP"),
                    CTR_Class = x.GetValue("CTR_Class"),
                    CHL_Class = x.GetValue("CHL_Class"),
                    CHL_Port = x.GetValueI("CHL_Port"),
                    CTR_ID_Cradle = x.GetValueNullI("MOD_CTR_Id")
                }).ToList();

                foreach (var agv in agvEntities)
                {
                    var newAgvCradle = new AgvCradleEntities();
                    if (agv.CTR_ID_Cradle != null)
                    {
                        agv.CradleEntities.AddRange(newAgvCradle.GetList(agv.CTR_ID_Cradle.Value));
                    }
                }
            }

            return agvEntities;
        }
    }
}