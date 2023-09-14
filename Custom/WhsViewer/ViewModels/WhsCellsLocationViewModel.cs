using AgilogDll;
using Caliburn.Micro;
using mSwAgilogDll;
using mSwAgilogDll.ViewModels;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Zebra.Sdk.Comm;

namespace WhsViewer.ViewModels
{
    [Export(typeof(WhsCellsLocationViewModel))]
    internal class WhsCellsLocationViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private AgilogDll.RunUtils _utils;
        private object _lockObj = new object();

        private AgilogDll.WhsCellsLocation _whsCellsLocation;
        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private bool _userPerm;
        private bool _isMag;
        private bool _isBuffer;

        #endregion

        #region Properties

        public bool HasLocation { get { return _whsCellsLocation != null; } }

        public AgilogDll.WhsCellsLocation Location
        {
            get { return _whsCellsLocation; }
            set
            {
                if (_whsCellsLocation != value)
                {
                    _whsCellsLocation = value;
                    NotifyOfPropertyChange(() => Location);

                    NotifyOfPropertyChange(() => HasLocation);
                    NotifyOfPropertyChange(() => CanSave);
                    NotifyOfPropertyChange(() => CanEditUDC);
                    NotifyOfPropertyChange(() => CanMove);
                    NotifyOfPropertyChange(() => CanImmediate);

                    IsMag = _whsCellsLocation != null && string.IsNullOrEmpty(_whsCellsLocation.LOC_MOD_Code);
                    IsBuffer = _whsCellsLocation != null && !string.IsNullOrEmpty(_whsCellsLocation.LOC_MOD_Code) && !string.IsNullOrEmpty(_whsCellsLocation.LOC_UDC_Code);
                }
            }
        }

        public List<WhsCfgLocationState> LocationStates { get { return BaseBindableDbEntity.GetList<WhsCfgLocationState>(DbUtils.CloneConnection(Global.Instance.ConnGlobal)); } }
        public bool CanSave { get { return (_whsCellsLocation != null) && _whsCellsLocation.CanSave; } }
        public bool CanEditUDC { get { return (_whsCellsLocation != null) && Global.Instance.CheckUserPerm(); } }
        public bool CanMove { get { return (_whsCellsLocation != null) && _whsCellsLocation.CanMove && Global.Instance.CheckUserPerm(); } }
        public bool CanImmediate { get { return (_whsCellsLocation != null) && _whsCellsLocation.CanImmediate; } }

        public bool CanTest { get { return Global.Instance.CheckUserPerm(); } }

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public bool UserPerm
        {
            get { return _userPerm; }
            set
            {
                _userPerm = value;
                NotifyOfPropertyChange(() => UserPerm);
            }
        }

        public bool IsMag
        {
            get { return _isMag; }
            set
            {
                _isMag = value;
                NotifyOfPropertyChange(() => IsMag);
            }
        }

        public bool IsBuffer
        {
            get { return _isBuffer; }
            set
            {
                _isBuffer = value;
                NotifyOfPropertyChange(() => IsBuffer);
            }
        }

        public string SnackBarMessage
        {
            get { return _SnackBarMessage; }
            set
            {
                // Forzo cambio di valore
                _SnackBarMessage = string.Empty;
                NotifyOfPropertyChange(() => SnackBarMessage);

                _SnackBarMessage = value;
                NotifyOfPropertyChange(() => SnackBarMessage);
            }
        }
        #endregion

        #region Constructor

        [ImportingConstructor]
        public WhsCellsLocationViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgilogDll.WhsCellsLocation whsCellsLocation = null)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _whsCellsLocation = whsCellsLocation;
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            UserPerm = Global.Instance.CheckUserPerm();           
        }

        #endregion

        #region Public methods

        public void EditUDC()
        {
            if (_whsCellsLocation == null) return;

            if (!Global.Instance.CheckUserPerm())
                return;

            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Maximized;
            settings.SizeToContent = SizeToContent.Manual;

            AgilogDll.ViewModels.UdcsViewModel vm = new AgilogDll.ViewModels.UdcsViewModel(
                _windowManager, _eventAggregator,
                _whsCellsLocation.LOC_UDC_Code,
                AgilogDll.ViewModels.EUdcPositions.Outside,
                _whsCellsLocation.LOC_UDT_Code,
                _whsCellsLocation.Cell.CEL_HGT_Num,
                _whsCellsLocation.Cell.CEL_WGH_Num,
                false, true);
            _windowManager.ShowDialogAsync(vm, settings: settings);

            if (vm.UdcSelection == null)
                return;

            _whsCellsLocation.LOC_UDC_Code = vm.UdcSelection.UDC_Code;
            _whsCellsLocation.LOC_LOS_Code = "O";
        }

        public async Task ToMagAsync()
        {
            if (_whsCellsLocation == null) return;

            if (!Global.Instance.CheckUserPerm())
                return;

            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Vuoi veramente rimandare il pallet a magazzino?"))) return;

            IsLoading = true;

            string error = await AgvReentranceAsync();

            IsLoading = false;

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Request sent successfully");
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

                    SELECT @UDC_Code = {_whsCellsLocation.Udc.UDC_Code.SqlFormat()}
                          ,@SourceMOD = {_whsCellsLocation.LOC_MOD_Code.SqlFormat()}

                    EXECUTE @RC = [dbo].[msp_MIS_GenMiss_AgvReentrance] 
                       @UDC_Code
                      ,@SourceMOD
                      ,@Error OUTPUT

                    SELECT @RC, @Error";

                var error = await Task.Run(() =>
                {
                    var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, false, out string err);
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

        public async Task DriveAsync()
        {
            if (_whsCellsLocation == null) return;

            if (!Global.Instance.CheckUserPerm())
                return;

            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to run a DRIVE command?"))) return;

            IsLoading = true;

            string error = await Common.DriveAsync(Global.Instance.ConnGlobal, _whsCellsLocation.Cell.STC_PLN_Code, cell: _whsCellsLocation.Cell.CEL_Id);

            IsLoading = false;

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Request sent successfully");
        }

        public async Task MoveAsync()
        {
            if (_whsCellsLocation == null) return;

            if (!Global.Instance.CheckUserPerm())
                return;

            var dialogSettings = new Dictionary<string, object>()
            {
                { "BorderThickness", new Thickness(1) },
                { "WindowState", WindowState.Normal },
                { "ResizeMode", ResizeMode.NoResize },
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                { "SizeToContent", SizeToContent.WidthAndHeight },
                { "Icon", Global.Instance.GetAppIcon() },
                { "MinHeight", 240 },
                { "MinWidth", 320 },
            };

            var vm = new SelectDestinationViewModel(_windowManager, _eventAggregator, _whsCellsLocation);
            var retVal = await _windowManager.ShowDialogAsync(vm, null, dialogSettings);

            if (!retVal.HasValue || !retVal.Value)
                return;

            long celId = vm.Channel;

            IsLoading = true;

            var row = await _utils.GenMiss_Move_UDCAsync(_whsCellsLocation.LOC_CEL_Id, _whsCellsLocation.LOC_Z, _whsCellsLocation.LOC_W, celId);

            IsLoading = false;

            if (row == null)
            {
                await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("An error occurred while trying to process your request. Check application logs"));
                return;
            }

            string error = row.GetValue(1);
            if (row.GetValueI(0) != 0 && !string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Request sent successfully");
        }

        public void Immediate()
        {
            if (_whsCellsLocation == null) return;

            Launcher.ImmediateWindow(_windowManager, _eventAggregator,
                _whsCellsLocation.LOC_UDC_Code,
                _whsCellsLocation.Udc.Compartments.Count > 0 ? _whsCellsLocation.Udc.Compartments[0].UCM_ITM_Code : null,
                _whsCellsLocation.Udc.Compartments.Count > 0 ? _whsCellsLocation.Udc.Compartments[0].UCM_Owner : -1,
                _whsCellsLocation.Udc.Compartments.Count > 0 && _whsCellsLocation.Udc.Compartments.Sum(c => c.UCM_Stock) > 0 ? _whsCellsLocation.Udc.Compartments.Sum(c => c.UCM_Stock) : 1,
                EImmediateOperations.Ship);
        }

        public async Task SaveAsync()
        {
            if (_whsCellsLocation == null) return;

            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to save changes to the selected location?")))
            {
                return;
            }

            IsLoading = true;

            bool retVal = await Task.Run(() => { return _whsCellsLocation.Update(); });

            IsLoading = false;

            if (!retVal)
            {
                await Global.ErrorAsync(_windowManager, _whsCellsLocation.LastError);
                return;
            }

            Location = _whsCellsLocation;

            SnackBarMessage = Global.Instance.LangTl("Location saved successfully");
        }

        public async Task TestAsync()
        {
            await Task.Delay(10);

            if (_whsCellsLocation == null) return;

            if (!await Global.ConfirmAsync(_windowManager, "Vuoi veramente lanciare il test su tutte le celle?"))
            {
                return;
            }

            IsLoading = true;

            try
            {
                DataRow row = null;

                // Eseguo la stored
                row = Test_EveryCells(_whsCellsLocation.LOC_UDC_Code, Common.Warehouse);
                int retVal = row.GetValueI("RetVal");
                if (retVal < 0)
                {
                    Logger.Log(row.GetValue("Error"), LogLevels.Fatal);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevels.Fatal);
            }

            IsLoading = false;

            Location = _whsCellsLocation;

            SnackBarMessage = "Test Iniziato";
        }

        #endregion

        #region Private Methods

        public DataRow Test_EveryCells(string Udc, string Warehouse)
        {
            DataRow row = null;
            string error = null;

            try
            {
                var query = $@"
                    DECLARE @RC int
                    DECLARE @Error nvarchar(255)

                    EXECUTE @RC = [dbo].[msp_WHS_CELLS_Test] 
                       {Udc.SqlFormat()}
                      ,{Warehouse.SqlFormat()}
                      ,@Error OUTPUT

                    SELECT @RC          AS 'RetVal'
                          ,@Error       AS 'Error'";

                var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, false, out error);
                if (dt != null && dt.Rows.Count > 0 && string.IsNullOrWhiteSpace(error))
                    row = dt.Rows[0];
                else
                {
                    row = null;
                }
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }

            return row;
        }

        #endregion
    }

    public class Launcher
    {
        public static void ImmediateWindow(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
            string udc,
            string item,
            int owner,
            decimal quantity = 0,
            EImmediateOperations? operation = null)
        {
            RunImmediateViewModel vm = new RunImmediateViewModel(
                windowManager, eventAggregator,
                operation, udc, item, owner, null, quantity);

            dynamic settings = new ExpandoObject();
            settings.Height = 600;
            settings.MinHeight = 300;
            settings.Width = 800;
            settings.MinWidth = 500;
            settings.Icon = GetImage("product_64px.png");
            settings.Title = "";
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Normal;
            settings.SizeToContent = SizeToContent.Manual;

            windowManager.ShowDialogAsync(vm, null, settings);
        }

        protected static ImageSource GetImage(string ImageName)
        {
            if (string.IsNullOrWhiteSpace(System.IO.Path.GetExtension(ImageName))) return null;

            string path = System.IO.Path.Combine(AppConfig.XmlRelativePath, AppConfig.GetXmlKeyValue("Graphics", "ImagesPublicPath"), ImageName);
            string uri = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(uri))
                uri = string.Format("pack://application:,,,/Resources/{0}", ImageName);

            BitmapImage source = new BitmapImage(new Uri(uri, UriKind.Absolute));

            return source;
        }
    }
}
