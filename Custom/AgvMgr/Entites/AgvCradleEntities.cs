using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AgvMgr.Entites
{
    public class AgvCradleEntities
    {
        public int CTR_ID { get; set; }
        public string CTR_Code { get; set; }
        public string CTR_MMG_Code { get; set; }
        public int CHL_Id { get; set; }
        public string CHL_IP { get; set; }
        public string CHL_Class { get; set; }
        public string CHL_Direction { get; set; }
        public int CHL_Port { get; set; }
        public string CTR_Class { get; set; }

        public List<AgvCradleEntities> GetList(int ctr_Id)
        {
            List<AgvCradleEntities> agvCradleEntities = new List<AgvCradleEntities>();

            string query = $"SELECT CTR_ID, CTR_Code, CTR_MMG_Code, CHL_Id, CHL_IP, CHL_Port, CTR_Class, CHL_Class, CHL_Direction " +
                           $"FROM MFC_CONTROLLERS " +
                           $"JOIN MFC_CHANNELS ON CTR_Id = CHL_CTR_Id " +
                           $"WHERE CTR_Id = {ctr_Id}";

            var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);

            if (dt.Rows.Count > 0)
            {
                agvCradleEntities = dt.AsEnumerable().Select(x => new AgvCradleEntities()
                {
                    CTR_ID = x.GetValueI("CTR_ID"),
                    CTR_Code = x.GetValue("CTR_Code"),
                    CTR_MMG_Code = x.GetValue("CTR_MMG_Code"),
                    CHL_Id = x.GetValueI("CHL_Id"),
                    CHL_IP = x.GetValue("CHL_IP"),
                    CTR_Class = x.GetValue("CTR_Class"),
                    CHL_Class = x.GetValue("CHL_Class"),
                    CHL_Port = x.GetValueI("CHL_Port"),
                    CHL_Direction = x.GetValue("CHL_Direction")
                }).ToList();

            }

            return agvCradleEntities;
        }
    }
}
