using DataStore.Server.Interfaces;
using mSwAgilogDll;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Data;
using System.Data.SqlClient;
using WebServiceSoapToyota;

namespace DataStore.Server.DatabaseStore
{
    //public partial class App : Application
    //{
    //    protected override void OnStartup(StartupEventArgs e)
    //    {
    //        var global = new Global(1402);

    //        if (!Global.Instance.Initialize(true))
    //        {
    //            Environment.Exit(0);
    //        }

    //        base.OnStartup(e);
    //    }
    //}
    public class DatabaseOperations : IOperations
    {
        //private const string filePath = @"C:\db.txt";
        //public RegisterResponseModel Register(RegisterRequestModel data)
        //{
        //    try
        //    { 
        //        File.AppendAllText(filePath, string.Format("{0},{1},{2} \n", data.Id, data.Name, data.EmailId));
        //        return new RegisterResponseModel()
        //        { 
        //            IsSuccess = true, 
        //            Code = 200, 
        //            Message = "SUCCESS"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new RegisterResponseModel(){
        //            IsSuccess = false,
        //            Code = 500,
        //            Message = ex.Message
        //        };
        //    }
        //}

        private readonly SqlConnection _conn;
        private RunUtils _utils;

        public DatabaseOperations()
        {
            var global = new Global(1402);

            if (!Global.Instance.Initialize(true))
            {
                Environment.Exit(0);
            }

            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));
        }
        public int TransportOrderStatusUpdate(string orderId, int statusId, WebServiceOrderRow[] orderRows, int palletType, string udcCode, int palletWidth, int palletHeight, int palletLenght, int palletWeight, int vehicleId, int errorCode, int palletAmount)
        {
            //ResponseModel<string> response = new ResponseModel<string>();
            //WebServiceOrderRow pippo;
            int retValue;
            try
            {
                SqlParameter OrderId = new SqlParameter("@OrderId", SqlDbType.NVarChar, 100);
                OrderId.SqlValue = orderId;

                SqlParameter Num = new SqlParameter("@Num", SqlDbType.Int);
                Num.SqlValue = 1;

                SqlParameter StatusId = new SqlParameter("@StatusId", SqlDbType.Int);
                StatusId.SqlValue = statusId;

                SqlParameter ErrorCode = new SqlParameter("@ErrorCode", SqlDbType.Int);
                ErrorCode.SqlValue = errorCode;

                SqlParameter VehicleId = new SqlParameter(@"VehicleId", SqlDbType.Int);
                VehicleId.SqlValue = vehicleId;

                SqlParameter ErrorSP = new SqlParameter("@Error", SqlDbType.NVarChar, 250);
                ErrorSP.SqlValue = "";


                retValue = (int)DbUtils.ExecuteStoredProcedure(
                "msp_Toyota_InsUpdOrder",
                      _conn, ref ErrorSP,
                      OrderId, Num, StatusId, ErrorCode, VehicleId);

                if (retValue == null || (int.Parse(retValue.ToString()) != 0) || (ErrorSP.Value != null && !string.IsNullOrWhiteSpace(ErrorSP.Value.ToString())))
                {
                    string errorDesc = (ErrorSP.Value != null && !string.IsNullOrWhiteSpace(ErrorSP.Value.ToString())) ? ErrorSP.Value.ToString() : "unkown error";
                    throw new Exception($"Error ");
                }
                else
                {
                    retValue = (int)ERetVal.OK;
                }

            }
            catch (Exception e)
            {
                retValue = (int)ERetVal.FAIL;
            }

            return retValue;
        }
    }
}
