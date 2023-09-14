using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMgr
{
    public class BayModule : BaseBindableObject, IDisposable
    {
        #region Members

        private DbConnection _connection;
        private DbConnection _spconnection;
        private int _bay;
        private string _module;
        private bool _moduleIsEnabled;
        private string _udc;
        private int _priority;
        private long? _mission;
        private string _emptyPalletImage = "/Resources/PalletVuoto.png";
        private string _palletImage = null;
        private string _fullPalletImage = "/Resources/PalletPieno.png";
        private int _ErrorCheck;
        private string _errUserAction = null;

        public delegate void BayModuleEventHandler(object sender, GenericEventArgs e);
        public event BayModuleEventHandler OnEnablingChanged;
        public event BayModuleEventHandler OnReentranceRequested;
        public event BayModuleEventHandler OnToBufferRequested;
        public event BayModuleEventHandler OnRemoveRequested;

        #endregion

        #region Properties

        public string PalletImage
        {
            get { return _palletImage; }
            set { SetProperty(ref _palletImage, value); }
        }

        public int Bay
        {
            get { return _bay; }
            set { SetProperty(ref _bay, value); }
        }

        public string Module
        {
            get { return _module; }
            set { SetProperty(ref _module, value); }
        }

        public bool ModuleIsEnabled
        {
            get { return _moduleIsEnabled; }
            set
            {
                SetProperty(ref _moduleIsEnabled, value);
                OnPropertyChanged("ModuleIsEnabledText");
                OnEnablingChanged?.Invoke(this, new GenericEventArgs(value));
            }
        }

        public string ModuleIsEnabledText
        {
            get { return _moduleIsEnabled ? "ABILITATO" : "DISABILITATO"; }
        }

        public string Udc
        {
            get { return _udc; }
            set { SetProperty(ref _udc, value); }
        }

        public int Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        public long? Mission
        {
            get { return _mission; }
            set
            {
                SetProperty(ref _mission, value);
                OnPropertyChanged("CanReenter");
                OnPropertyChanged("CanRemove");
            }
        }

        public bool CanReenter
        {
            get { return Mission.HasValue; }
        }

        public bool CanRemove
        {
            get { return Mission.HasValue; }
        }

        public string ErrUserAction
        {
            get { return _errUserAction; }
            set 
            { 
                SetProperty(ref _errUserAction, value);
                if (string.IsNullOrWhiteSpace(ErrUserAction))
                {
                    ErrorCheck = 0;
                }
                else { ErrorCheck = 1; }
            }
        }

        public int ErrorCheck
        {
            get { return _ErrorCheck; }
            set { SetProperty(ref _ErrorCheck, value); }
        }
        #endregion

        #region Constructor

        public BayModule(DbConnection connection) : base()
        {
            _connection = DbUtils.CloneConnection(connection);
            _spconnection = DbUtils.CloneConnection(connection);
        }

        ~BayModule()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection = null;
            }
        }

        #endregion

        #region Public Methods

        public void RevertEnabling()
        {
            _moduleIsEnabled = !_moduleIsEnabled;
            OnPropertyChanged("ModuleIsEnabled");
            OnPropertyChanged("ModuleIsEnabledText");
        }

        public async Task Refresh()
        {
            try
            {
                await Task.Run(() =>
                {
                    // Refresh missione a bordo
                    var query = $@"SELECT MIS_Id, MIS_UDC_Code, MIS_ERR_UserAction
                        FROM MIS_MISSIONS
                        WHERE MIS_Current_MOD_Code = {Module.SqlFormat()}";

                    var dt = DbUtils.ExecuteDataTable(query, _connection, true);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        Mission = null;
                        Udc = null;
                        ErrUserAction = null;
                        PalletImage = null;
                    }
                    else
                    {
                        Mission = dt.Rows[0].GetValueL("MIS_Id");
                        Udc = dt.Rows[0].GetValue("MIS_UDC_Code");
                        ErrUserAction = dt.Rows[0].GetValue("MIS_ERR_UserAction");
                        PalletImage = _fullPalletImage;
                    }
                });
            }
            catch { }
        }

        public async Task<string> AgvReentranceAsync()
        {
            try
            {
                var query = $@"
                    DECLARE @RC int
                    DECLARE @UDC_Code nvarchar(50)
                    DECLARE @SourceMOD nvarchar(50)
                    DECLARE @Error nvarchar(255)

                    SELECT @UDC_Code = {Udc.SqlFormat()}
                          ,@SourceMOD = {Module.SqlFormat()}

                    EXECUTE @RC = [dbo].[msp_MIS_GenMiss_AgvReentrance] 
                       @UDC_Code
                      ,@SourceMOD
                      ,@Error OUTPUT

                    SELECT @RC, @Error";

                var error = await Task.Run(() =>
                {
                    var dt = DbUtils.ExecuteDataTable(query, _spconnection, false, out string err);
                    if (!string.IsNullOrWhiteSpace(err)) return err;

                    return dt.Rows[0].GetValue(1);
                });

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AgvToBufferAsync()
        {
            try
            {
                var query = $@"
                    DECLARE @RC int
                    DECLARE @Error nvarchar(255)
                    DECLARE @Position nvarchar(50)
                    DECLARE @Mis_Id bigint

                    SELECT @Mis_Id = {Mission.SqlFormat()}
                          ,@Position = {Module.SqlFormat()}

                    EXECUTE @RC = [dbo].[msp_MIS_FindBuffer_Sodico] 
                       @Error OUTPUT
                      ,@Position
                      ,@Mis_Id

                    SELECT @RC, @Error";

                var error = await Task.Run(() =>
                {
                    var dt = DbUtils.ExecuteDataTable(query, _spconnection, false, out string err);
                    if (!string.IsNullOrWhiteSpace(err)) return err;

                    return dt.Rows[0].GetValue(1);
                });

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoveMissionAsync()
        {
            try
            {
                if (!Mission.HasValue) return "Missione non trovata";

                var query = $@"
                    UPDATE MIS_MISSIONS
                    SET MIS_L1_MST_Code = 'END'
                    WHERE MIS_Id = {Mission.SqlFormat()}";

                var error = await Task.Run(() =>
                {
                    DbUtils.ExecuteNonQuery(query, _spconnection, false, out string err);
                    if (!string.IsNullOrWhiteSpace(err)) return err;

                    return null;
                });

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Events

        public void Reenter()
        {
            OnReentranceRequested?.Invoke(this, new GenericEventArgs());
        }

        public void ToBuffer()
        {
            OnToBufferRequested?.Invoke(this, new GenericEventArgs());
        }

        public void Remove()
        {
            OnRemoveRequested?.Invoke(this, new GenericEventArgs());
        }

        #endregion

        #region Static

        public static List<BayModule> GetList(DbConnection conn, int bay, out string error)
        {
            var list = new List<BayModule>();
            error = null;

            try
            {
                var query = $@"
                    SELECT [BMO_BAY_Num]
                          ,[BMO_MOD_Code]
                          ,[BMO_Priority]
                          ,[BMO_Capacity]
                          ,[BMO_Connected_Bay]
                      FROM [HND_BAY_MODULES]
                    WHERE [BMO_BAY_Num] = {bay}";

                var dt = DbUtils.ExecuteDataTable(query, conn, false, out error);
                if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);

                foreach (DataRow row in dt.Rows)
                {
                    var bayModule = new BayModule(conn);

                    bayModule.Bay = row.GetValueI("BMO_BAY_Num");
                    bayModule.Module = row.GetValue("BMO_MOD_Code");
                    bayModule._moduleIsEnabled = row.GetValueI("BMO_Capacity") > 0;
                    bayModule.Priority = row.GetValueI("BMO_Priority");
                    bayModule.Udc = null;
                    bayModule.Mission = null;

                    list.Add(bayModule);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                list = null;
            }

            return list;
        }

        #endregion
    }
}
