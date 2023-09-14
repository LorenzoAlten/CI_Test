using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhsViewer.ViewModels
{
    [Export(typeof(FloorViewModel))]
    class FloorViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _Warehouse;
        private object _lockObj = new object();

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private int _Aisle;        
        private int _Level;
        private bool _EnabledIN;
        private bool _EnabledOUT;
        private decimal _FillingPercentage;
        private decimal _UnusablePercentage;
        private int _CellsToRecompact;

        #endregion

        #region Properties
        public int Aisle
        {
            get { return _Aisle; }
            set
            {
                if (_Aisle != value)
                {
                    _Aisle = value;
                    NotifyOfPropertyChange(() => Aisle);

                    LoadData();
                    LoadStatistics();
                }
            }
        }

        public int CurrentLevel
        {
            get { return _Level; }
            set
            {
                if (_Level != value)
                {
                    _Level = value;
                    NotifyOfPropertyChange(() => CurrentLevel);

                    LoadData();
                    LoadStatistics();
                }
            }
        }

        public bool EnabledIN
        {
            get { return _EnabledIN; }
            set
            {
                _EnabledIN = value;
                NotifyOfPropertyChange(() => EnabledIN);
            }
        }

        public bool EnabledOUT
        {
            get { return _EnabledOUT; }
            set
            {
                _EnabledOUT = value;
                NotifyOfPropertyChange(() => EnabledOUT);
            }
        }

        public decimal FillingPercentage
        {
            get { return _FillingPercentage; }
            set
            {
                _FillingPercentage = value;
                NotifyOfPropertyChange(() => FillingPercentage);
            }
        }

        public decimal UnusablePercentage
        {
            get { return _UnusablePercentage; }
            set
            {
                _UnusablePercentage = value;
                NotifyOfPropertyChange(() => UnusablePercentage);
            }
        }

        public int CellsToRecompact
        {
            get { return _CellsToRecompact; }
            set
            {
                _CellsToRecompact = value;
                NotifyOfPropertyChange(() => CellsToRecompact);
            }
        }

        public bool HasPermission { get { return Global.Instance.CheckUserPerm(); } }

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
        public FloorViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, string warehouse, int aisle, int level)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _Warehouse = warehouse;
            _Aisle = aisle;
            _Level = level;
        }

        #endregion

        #region ViewModel Override
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            LoadData();

            LoadStatistics();
        } 
        #endregion

        #region Public methods
        public async Task SaveAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to change the current Floor enabling states?")))
            {
                return;
            }

            IsLoading = true;

            string error = await Task.Run(() =>
            {
                return AgilogDll.WarehouseCellMgr.Ptr.ChangeLevelEnabledStatus(_Warehouse, _Aisle, _Level, EnabledIN, EnabledOUT);
            });

            IsLoading = false;

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Floor state saved successfully");

            AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();

            await _eventAggregator.PublishOnUIThreadAsync($"CELL");
        }
        #endregion

        #region Private methods
        private void LoadData()
        {
            Task.Factory.StartNew(() =>
            {
                var cells = AgilogDll.WarehouseCellMgr.Ptr.GetCells(_Warehouse, _Aisle, _Level);

                EnabledIN = cells.Any(c => c.EnabledIN);
                EnabledOUT = cells.Any(c => c.EnabledOUT);
            });
        }

        private void LoadStatistics()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string query = $@"SELECT * FROM [dbo].[mfn_WHS_AisleLevelGetStatistics] ({_Warehouse.SqlFormat()}, {_Aisle.SqlFormat()}, {_Level.SqlFormat()})";
                    DataTable dt = DbUtils.ExecuteDataTable(query, DbUtils.CloneConnection(Global.Instance.ConnGlobal));

                    FillingPercentage = dt.Rows[0].GetValueDec("Filling");
                    UnusablePercentage = dt.Rows[0].GetValueDec("Unusable");
                    CellsToRecompact = dt.Rows[0].GetValueI("Recompactable");
                }
                catch
                {
                    FillingPercentage = int.MinValue;
                    UnusablePercentage = int.MinValue;
                    CellsToRecompact = int.MinValue;
                }
            });
        } 
        #endregion
    }
}
