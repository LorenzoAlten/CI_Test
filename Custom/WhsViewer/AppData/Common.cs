using mSwDllUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WhsViewer
{
    public static class Common
    {
        private static int _aisle;
        private static int _level;
        private static int _rack;
        private static double _zoomFactor;

        public static string Warehouse { get; set; }
        public static int Aisle
        {
            get { return _aisle; }
            set
            {
                AisleChanged = _aisle != value;
                _aisle = value;
            }
        }
        public static int Level
        {
            get { return _level; }
            set
            {
                LevelChanged = _level != value;
                _level = value;
            }
        }
        public static int Rack
        {
            get { return _rack; }
            set
            {
                RackChanged = _rack != value;
                _rack = value;
            }
        }
        public static double ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                ZoomFactorChanged = _zoomFactor != value;
                _zoomFactor = value;
            }
        }
        public static bool AisleChanged { get; set; }
        public static bool LevelChanged { get; set; }
        public static bool RackChanged { get; set; }
        public static bool ZoomFactorChanged { get; set; }
        public static int StcController { get; set; }

        public static async Task<string> DriveAsync(DbConnection conn, string machine, string module = null, long? cell = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = $@"
                        DECLARE @RC int
                        DECLARE @PLN_Code nvarchar(50)
                        DECLARE @Dest_MOD_Code nvarchar(50)
                        DECLARE @Dest_CEL_Id bigint
                        DECLARE @Error nvarchar(255)

                        SELECT @PLN_Code = {machine.SqlFormat()}
                              ,@Dest_MOD_Code = {module.SqlFormat()}
                              ,@Dest_CEL_Id = {cell.SqlFormat()}

                        EXECUTE @RC = [dbo].[msp_MIS_GenMiss_Drive] 
                           @PLN_Code
                          ,@Dest_MOD_Code
                          ,@Dest_CEL_Id
                          ,@Error OUTPUT
                    
                        SELECT @RC, @Error";
                    var result = DbUtils.ExecuteDataTable(query, conn, false, out string error);
                    if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);
                    error = result.Rows[0].GetValue(1);
                    if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);
                }
                catch (Exception ex) { return ex.Message; }
                return null;
            });
        }
    }

    public class CompactableCell
    {
        public string Warehouse { get; set; }
        public int Aisle { get; set; }
        public int Rack { get; set; }
        public long CEL_Id { get; set; }
        public int CEL_X { get; set; }
        public int CEL_Y { get; set; }

        public static List<CompactableCell> GetList(SqlConnection connection, string warehouse, int aisle)
        {
            List<CompactableCell> cells = null;

            try
            {
                var query = $@"
                    SELECT * FROM [dbo].[mfn_WHS_CELLS_GetCompactableCells] ()
                    WHERE Warehouse = {warehouse.SqlFormat()}
                    AND Aisle = {aisle.SqlFormat()}
                    ORDER BY Rack, CEL_Y";
                var dt = DbUtils.ExecuteDataTable(query, connection);
                cells = new List<CompactableCell>();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        cells.Add(new CompactableCell
                        {
                            Warehouse = row.GetValue("Warehouse"),
                            Aisle = row.GetValueI("Aisle"),
                            Rack = row.GetValueI("Rack"),
                            CEL_Id = row.GetValueL("CEL_Id"),
                            CEL_X = row.GetValueI("CEL_X"),
                            CEL_Y = row.GetValueI("CEL_Y"),
                        });
                    }
                }
            }
            catch
            {
                cells = new List<CompactableCell>();
            }
            return cells;
        }
    }

    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return value.ToString().TrimUI();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
