using mSwAgilogDll;
using mSwDllUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WhsViewer
{
    public class StcBay : HndBayStcbay
    {
        #region Members

        protected string _STC_ASL_WHS_Code;
        protected int _STC_ASL_Num;

        #endregion

        #region Properties

        public string STC_ASL_WHS_Code
        {
            get { return _STC_ASL_WHS_Code; }
            set { SetProperty(ref _STC_ASL_WHS_Code, value); }
        }

        public int STC_ASL_Num
        {
            get { return _STC_ASL_Num; }
            set { SetProperty(ref _STC_ASL_Num, value); }
        }

        public string Name
        {
            get { return $"{BIN_Desc} - {string.Join(", ", Modules.ToArray())}"; }
        }

        #endregion

        #region Constructor

        public StcBay(SqlConnection connection) : base(connection) { }

        public StcBay(SqlConnection connection, DataRow row) : base(connection, row) { }

        #endregion

        #region Override

        protected override void LoadFromDataRow(DataRow Row)
        {
            base.LoadFromDataRow(Row);

            STC_ASL_WHS_Code = Row.GetValue("STC_ASL_WHS_Code");
            STC_ASL_Num = Row.GetValueI("STC_ASL_Num");
        }

        #endregion

        #region Public

        /// <summary>
        /// Invoca il Drive per il primo fra i moduli associati alla baia
        /// </summary>
        /// <returns></returns>
        public async Task<string> DriveAsync()
        {
            if (Modules.Count <= 0) throw new Exception("No physical position found for selected bay");

            return await Common.DriveAsync(_Conn, BIN_STC_PLN_Code, Modules.First());
        }

        #endregion

        #region Static

        public static List<StcBay> GetList(SqlConnection connection, string condition = null, bool keepConnectionOpen = false)
        {
            List<StcBay> list = new List<StcBay>();

            try
            {
                var query = $@"
                    SELECT [BIN_Num]
                          ,[BIN_Desc]
                          ,[BIN_STC_PLN_Code]
                          ,[BIN_Load]
                          ,[BIN_Unload]
                          ,[BIN_X]
                          ,[BIN_Y]
                          ,[BIN_CradleMask]
                          ,[BIN_Modules]
                          ,[BIN_Capacity]
                          ,[BIN_LinkedBays]
                          ,[STC_ASL_WHS_Code]
                          ,[STC_ASL_Num]
                      FROM [HND_BAY_STCBAYS]
                      INNER JOIN [STC_MACHINES] ON [STC_PLN_Code] = [BIN_STC_PLN_Code]
                      {condition}";
                var dt = DbUtils.ExecuteDataTable(query, connection);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new StcBay(connection, row));
                }
            }
            catch
            {
                list = null;
            }

            return list;
        }

        #endregion
    }
}
