using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyotaClientWebServiceSoapMgr
{
    public sealed class Manager
    {
        #region Singleton

        private static readonly Manager instance = new Manager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Manager()
        {
        }

        private Manager()
        {
        }

        public static Manager Instance
        {
            get { return instance; }
        }

        #endregion

        internal List<Item> ItemsList;
        internal List<BusinessPartner> CustomersList;

        public bool Standalone { get; internal set; } = false;

        public DataTable GetCompartmentsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RowNr", typeof(int));
            dt.Columns.Add("UDC_Code", typeof(string));
            dt.Columns.Add("Index", typeof(int));
            return dt;
        }

        /// <summary>
        /// Crea una nuova UDC
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Customer"></param>
        /// <param name="CustomerOrderNo"></param>
        /// <param name="UdcCode"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public bool CreateUdc(string UDT_Code, string OrderNo, string Customer, string CustomerOrderNo, DataTable Compartments, ref string UdcCode, ref string Error)
        {
            var Exc = "";
            var pUDT_Code = new SqlParameter("@UDC_UDT_Code", (object)UDT_Code ?? DBNull.Value);
            var pOrderNo = new SqlParameter("@UDC_OrderNo", (object)OrderNo ?? DBNull.Value);
            var pCustomerCode = new SqlParameter("@UDC_BPA_Code", !string.IsNullOrWhiteSpace(Customer) ? (object)Customer : DBNull.Value);
            var pCustomerOrderNo = new SqlParameter("@UDC_CustomerOrderNo", !string.IsNullOrWhiteSpace(CustomerOrderNo) ? (object)CustomerOrderNo : DBNull.Value);
            var pCompartmentsTable = new SqlParameter("@CompartmentsTable", Compartments);
            var pUdcCode = new SqlParameter("@UDC_Code", SqlDbType.NVarChar, 50);
            var pError = new SqlParameter("@Error", SqlDbType.NVarChar, 500);
            var output = new SqlParameter[] { pUdcCode, pError };

            var retVal = DbUtils.ExecuteStoredProcedure("msp_UDC_NewToyotaServiceSoap", (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), ref Exc, ref output,
                pUDT_Code, pOrderNo, pCustomerCode, pCustomerOrderNo, pCompartmentsTable);

            UdcCode = pUdcCode.Value.ToString();
            Error = pError.Value.ToString();

            return retVal != null && ((int)retVal == 0);
        }

        public void LoadArchives()
        {
            try
            {
                LoadCustomersList();

                LoadItemsList();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Carica la lista di articoli
        /// </summary>
        internal void LoadItemsList()
        {
            //Task.Run(() =>
            // {
                 if (ItemsList == null)
                     ItemsList = BaseBindableDbEntity.GetList<Item>((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), "WHERE ITM_Code <> [dbo].[mfn_Params_GetStr] ('ITM_Dummy', '_', '')").ToList();
             //});
        }

        /// <summary>
        /// Carica la lista dei clienti
        /// </summary>
        internal void LoadCustomersList()
        {
            //Task.Run(() =>
            //{
                if (CustomersList == null)
                    CustomersList = BaseBindableDbEntity.GetList<BusinessPartner>((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal)).ToList();
                //});
        }
    }
}
