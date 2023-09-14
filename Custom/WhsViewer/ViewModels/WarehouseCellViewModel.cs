using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WhsViewer.ViewModels
{
    [Export(typeof(WarehouseCellViewModel))]
    class WarehouseCellViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private RunUtils _utils;
        private object _lockObj = new object();

        private AgilogDll.WarehouseCell _warehouseCell;
        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private bool _userPerm;

        #endregion

        #region Properties

        public bool HasCell { get { return _warehouseCell != null; } }

        public AgilogDll.WarehouseCell WarehouseCell
        {
            get { return _warehouseCell; }
            set
            {
                if (_warehouseCell != value)
                {
                    _warehouseCell = value;
                    NotifyOfPropertyChange(() => WarehouseCell);

                    NotifyOfPropertyChange(() => HasCell);
                    NotifyOfPropertyChange(() => EnabledIN);
                    NotifyOfPropertyChange(() => EnabledOUT);
                }
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

        public bool CanRecompact
        {
            get { return _warehouseCell != null && _warehouseCell.Recompactable; }
        }

        public bool EnabledIN
        {
            get { return _warehouseCell != null && _warehouseCell.EnabledIN; }
            set
            {
                if (_warehouseCell == null) return;

                if (value != _warehouseCell.EnabledIN)
                {
                    bool? enable = null;
                    string message = null;

                    if (value)
                    {
                        enable = true;
                        message = Global.Instance.LangTl("Do you really want to ENABLE all locations for IN?");
                    }
                    else
                    {
                        enable = false;
                        message = Global.Instance.LangTl("Do you really want to DISABLE all locations for IN?");
                    }

                    //Antonio
                    //if (!Global.ConfirmAsync(_windowManager, message))
                    //{
                    //    return;
                    //}

                    IsLoading = true;

                    string error = _warehouseCell.Enable(enable, null);

                    IsLoading = false;

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Global.ErrorAsync(_windowManager, error);
                        return;
                    }

                    SnackBarMessage = Global.Instance.LangTl("Cell enabling changed successfully");

                    NotifyOfPropertyChange(() => EnabledIN);

                    AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();

                    _eventAggregator.PublishOnUIThreadAsync($"CELL");
                }
            }
        }

        public bool EnabledOUT
        {
            get { return _warehouseCell != null && _warehouseCell.EnabledOUT; }
            set
            {
                if (_warehouseCell == null) return;

                if (value != _warehouseCell.EnabledOUT)
                {
                    bool? enable = null;
                    string message = null;

                    if (value)
                    {
                        enable = true;
                        message = Global.Instance.LangTl("Do you really want to ENABLE all locations for OUT?");
                    }
                    else
                    {
                        enable = false;
                        message = Global.Instance.LangTl("Do you really want to DISABLE all locations for OUT?");
                    }
                    //Antonio
                    //if (!Global.ConfirmAsync(_windowManager, message))
                    //{
                    //    return;
                    //}

                    IsLoading = true;

                    string error = _warehouseCell.Enable(null, enable);

                    IsLoading = false;

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Global.ErrorAsync(_windowManager, error);
                        return;
                    }

                    SnackBarMessage = Global.Instance.LangTl("Cell enabling changed successfully");

                    NotifyOfPropertyChange(() => EnabledOUT);

                    AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();

                    _eventAggregator.PublishOnUIThreadAsync($"CELL");
                }
            }
        }

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
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
        public WarehouseCellViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgilogDll.WarehouseCell warehouseCell = null)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _warehouseCell = warehouseCell;
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

UserPerm = Global.Instance.CheckUserPerm();
        }

        #endregion

        #region Public methods

        public async Task RecompactAsync()
        {
            if (_warehouseCell == null)
                return;

            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to launch the operation for the selected cell?")))
                return;

            IsLoading = true;

            var row = await _utils.Cells_RecompactAsync(Common.Warehouse, Common.Aisle, _warehouseCell.CEL_Id);

            IsLoading = false;

            string error = row.GetValue(1);

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Cell operation launched successfully");
        } 

        #endregion
    }
}
