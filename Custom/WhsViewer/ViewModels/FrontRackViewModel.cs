using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WhsViewer.ViewModels
{
    [Export(typeof(FrontRackViewModel))]
    class FrontRackViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _Warehouse;
        private object _lockObj = new object();

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private int _AisleNum;
        private int _RackNum;

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
                }
            }
        }

        public AgilogDll.WhsAisle CurrentAisle
        {
            get { return AgilogDll.WarehouseCellMgr.Ptr.Warehouses[_Warehouse].Aisles[_AisleNum]; }
        }

        public int RackNum
        {
            get { return _RackNum; }
            set
            {
                if (_RackNum != value)
                {
                    _RackNum = value;
                    NotifyOfPropertyChange(() => RackNum);
                    NotifyOfPropertyChange(() => CurrentRack);

                    LoadDataAsync();
                }
            }
        }

        public AgilogDll.WhsRack CurrentRack
        {
            get
            {
                if (CurrentAisle != null && CurrentAisle.Racks.ContainsKey(_RackNum))
                    return CurrentAisle.Racks[_RackNum];

                return null;
            }
        }

        public ObservableCollection<Column> Columns { get; set; } = new ObservableCollection<Column>();

        private List<int> _RowNr = new List<int>();
        public List<int> RowNr
        {
            get { return _RowNr; }
            set
            {
                if (_RowNr != value)
                {
                    _RowNr = value;
                    NotifyOfPropertyChange(() => RowNr);
                }
            }
        }

        public AgilogDll.WarehouseCell SelectedCell { get; private set; }

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
        public FrontRackViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, string warehouse, int aisle, int rack)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _Warehouse = warehouse;
            _AisleNum = aisle;
            _RackNum = rack;

            BindingOperations.EnableCollectionSynchronization(Columns, _lockObj);
        }

        #endregion

        #region ViewModel Override
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            LoadDataAsync();
        }
        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);
        }


        #endregion

        #region Private methods
        private async Task LoadDataAsync()
        {
            IsLoading = true;

            BindingOperations.EnableCollectionSynchronization(Columns, _lockObj);

            await Task.Factory.StartNew(() =>
            {
                Columns.Clear();
                RowNr = null;

                var cells = GetCells();

                if (cells.Count > 0)
                {
                    RowNr = cells.Select(x => x.WarehouseCell.Y).Distinct().OrderByDescending(x => x).ToList();

                    var columns = cells.Select(x => x.WarehouseCell.X).Distinct().OrderBy(x => x);

                    for (int i = 1; i <= columns.Count(); i++)
                    {
                        Columns.Add(new Column() { Index = i, Rows = cells.Where(x => x.WarehouseCell.X == i).OrderByDescending(x => x.WarehouseCell.Y).ToList() });
                    }
                }
            });

            IsLoading = false;
        }

        private List<CellViewModel> GetCells()
        {
            List<CellViewModel> cells = new List<CellViewModel>();

            var tmpCells = AgilogDll.WarehouseCellMgr.Ptr.GetCells(_Warehouse, _AisleNum).Where(x => x.CEL_RCK_Num == _RackNum);

            foreach (var cell in tmpCells)
            {
                cells.Add(new CellViewModel(cell));
            }

            return cells;
        }

        #endregion

        #region Public methods
        public void Select(CellViewModel cell)
        {
            if (SelectedCell != null) SelectedCell.IsSelected = false;

            cell.WarehouseCell.IsSelected = true;

            SelectedCell = cell.WarehouseCell;

            _eventAggregator.PublishOnUIThreadAsync($"CELL");
        }
        #endregion
    }
}

public class Column
{
    #region Properties
    public int Index { get; set; }
    public List<CellViewModel> Rows { get; set; } = new List<CellViewModel>();
    #endregion
}

[Export(typeof(CellViewModel))]
public class CellViewModel : Screen
{
    #region Properties
    public AgilogDll.WarehouseCell WarehouseCell { get; private set; }
    public bool HasLocations { get { return WarehouseCell != null && WarehouseCell.Locations.Count() > 0; } }
    public bool HasUDC { get { return WarehouseCell != null && HasLocations && WarehouseCell.Locations.Any(x => !string.IsNullOrEmpty(x.LOC_UDC_Code)); } }
    #endregion

    #region Constructor
    public CellViewModel(AgilogDll.WarehouseCell warehouseCell)
    {
        WarehouseCell = warehouseCell;
    }
    #endregion
}