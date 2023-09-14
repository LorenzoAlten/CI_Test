using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhsViewer.ViewModels
{
    [Export(typeof(AisleViewModel))]
    class AisleViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _Warehouse;
        private object _lockObj = new object();

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private int _AisleNum;
        private decimal _FillingPercentage;
        private decimal _UnusablePercentage;
        private int _CellsToRecompact;
        private List<CompactableCell> _compactableCells;
        private int _compactableIndex;
        private CompactableCell _compactableCell;

        #endregion

        #region Properties

        public int AisleNum
        {
            get { return _AisleNum; }
            set
            {
                if (_AisleNum != value)
                {
                    _AisleNum = value;
                    NotifyOfPropertyChange(() => AisleNum);

                    NotifyOfPropertyChange(() => CurrentAisle);
                    NotifyOfPropertyChange(() => EnabledIN);
                    NotifyOfPropertyChange(() => EnabledOUT);

                    LoadStatistics();
                }
            }
        }

        public AgilogDll.WhsAisle CurrentAisle
        {
            get { return AgilogDll.WarehouseCellMgr.Ptr.Warehouses[_Warehouse].Aisles[_AisleNum]; }
        }

        public bool EnabledIN
        {
            get { return CurrentAisle.ASL_EnabledIN; }
            set
            {
                CurrentAisle.ASL_EnabledIN = value;
                NotifyOfPropertyChange(() => EnabledIN);
            }
        }

        public bool EnabledOUT
        {
            get { return CurrentAisle.ASL_EnabledOUT; }
            set
            {
                CurrentAisle.ASL_EnabledOUT = value;
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

        public bool HasCompactableCells
        {
            get
            {
                return _compactableCells != null &&
                       _compactableCells.Count > 0;
            }
        }

        public bool CanNavigateBack
        {
            get { return HasCompactableCells; }
        }

        public bool CanNavigateForward
        {
            get { return HasCompactableCells; }
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
        public AisleViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, string warehouse, int aisle)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _Warehouse = warehouse;
            _AisleNum = aisle;
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
            if (! await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to change the current Aisle enabling states?")))
            {
                return;
            }

            IsLoading = true;

            bool retVal = await Task.Run(() =>
            {
                return CurrentAisle.Update();
            });

            IsLoading = false;

            if (!retVal)
            {
                await Global.ErrorAsync(_windowManager, CurrentAisle.LastError);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Aisle state saved successfully");

            AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();

            await _eventAggregator.PublishOnUIThreadAsync($"CELL");
        }

        public void NavigateBack()
        {
            _compactableIndex--;
            if (_compactableIndex < 0) _compactableIndex = _compactableCells.Count - 1;

            _compactableCell = _compactableCells[_compactableIndex];

            SelectCompactableCell();
        }

        public void NavigateForward()
        {
            _compactableIndex++;
            if (_compactableIndex >= _compactableCells.Count) _compactableIndex = 0;

            _compactableCell = _compactableCells[_compactableIndex];

            SelectCompactableCell();
        }

        #endregion

        #region Private methods
        private void LoadData()
        {
            if (_compactableCells == null)
            {
                _compactableCells = CompactableCell.GetList((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), _Warehouse, _AisleNum);
                _compactableIndex = -1;
                _compactableCell = null;
            }
        }

        private void LoadStatistics()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string query = $@"SELECT * FROM [dbo].[mfn_WHS_AisleGetStatistics] ({_Warehouse.SqlFormat()}, {_AisleNum.SqlFormat()})";
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

        private void SelectCompactableCell()
        {
            //if (_compactableCell == null) return;

            //WarehouseCellMgr.Ptr.DeselectAll();

            //if (_compactableCell.Warehouse == Common.Warehouse &&
            //    _compactableCell.Aisle == Common.Aisle &&
            //    _compactableCell.CEL_Y == Common.Level)
            //{
            //    var warehouseCell = CellsCollectionRack1.Union(CellsCollectionRack2).FirstOrDefault(c => c.CEL_Id == _compactableCell.CEL_Id);
            //    if (warehouseCell == null) return;

            //    warehouseCell.IsSelected = true;
            //    SelectedCell = warehouseCell;
            //    warehouseCell.Locations.First().IsSelected = true;
            //    SelectedLocation = warehouseCell.Locations.First();
            //}
            //else
            //{
            //    Common.Aisle = _compactableCell.Aisle;
            //    Common.Level = _compactableCell.CEL_Y;

            //    WarehouseCellMgr.Ptr.DeselectAll();
            //    SelectedLocation = null;

            //    RefreshAll();
            //}
        } 
        #endregion
    }
}
