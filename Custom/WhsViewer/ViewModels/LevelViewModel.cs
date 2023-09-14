using mSwAgilogDll;
using mSwAgilogDll.ViewModels;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Threading;

namespace WhsViewer.ViewModels
{
    [Export(typeof(LevelViewModel))]
    class LevelViewModel : Conductor<Screen>, IHandle<string>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private RunUtils _utils;
        private object _lockObj = new object();

        private bool _IsLoading = false;
        private bool _FilterToggleButtonIsChecked;
        private string _SnackBarMessage;
        private Orientation _CellOrientation;

        private AgilogDll.WarehouseCell _selectedCell;
        private AgilogDll.WhsCellsLocation _selectedLocation;
        private WarehouseCellViewModel _Cell;
        private WhsCellsLocationViewModel _Location;
        private AisleViewModel _Aisle;
        private FloorViewModel _Floor;
        private BayViewModel _Bays;
        private FrontRackViewModel _Rack;

        private List<CompactableCell> _compactableCells;
        private int _compactableIndex;
        private CompactableCell _compactableCell;

        private int _currentAisle;
        private int _currentLevel;
        private string _currentAisleName;
        private bool _userPerm;

        #endregion

        #region Properties

        public FilterViewModel Filter { get; private set; }

        public ObservableCollection<AgilogDll.WarehouseCell> CellsCollectionRack1 { get; set; } = new ObservableCollection<AgilogDll.WarehouseCell>();

        public ObservableCollection<AgilogDll.WarehouseCell> CellsCollectionRack2 { get; set; } = new ObservableCollection<AgilogDll.WarehouseCell>();

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

        public bool FilterToggleButtonIsChecked
        {
            get { return _FilterToggleButtonIsChecked; }
            set
            {
                _FilterToggleButtonIsChecked = value;
                NotifyOfPropertyChange(() => FilterToggleButtonIsChecked);
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

        public AisleViewModel Aisle
        {
            get { return _Aisle; }
            set
            {
                _Aisle = value;
                NotifyOfPropertyChange(() => Aisle);
            }
        }

        public FloorViewModel Floor
        {
            get { return _Floor; }
            set
            {
                _Floor = value;
                NotifyOfPropertyChange(() => Floor);
            }
        }

        public BayViewModel Bays
        {
            get { return _Bays; }
            set
            {
                _Bays = value;
                NotifyOfPropertyChange(() => Bays);
            }
        }

        public FrontRackViewModel Rack
        {
            get { return _Rack; }
            set
            {
                _Rack = value;
                NotifyOfPropertyChange(() => Rack);
            }
        }

        public AgilogDll.WarehouseCell SelectedCell
        {
            get { return _selectedCell; }
            set
            {
                _selectedCell = value;
                NotifyOfPropertyChange(() => SelectedCell);

                if (_selectedCell != null)
                {
                    Cell.WarehouseCell = _selectedCell;
                }
                else
                    Cell.WarehouseCell = null;
            }
        }

        public WarehouseCellViewModel Cell
        {
            get { return _Cell; }
            set
            {
                _Cell = value;
                NotifyOfPropertyChange(() => Cell);
            }
        }

        public WhsCellsLocationViewModel Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                NotifyOfPropertyChange(() => Location);
            }
        }

        public AgilogDll.WhsCellsLocation SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;

                if (_selectedLocation == null)
                {
                    Location.Location = null;
                    SelectedCell = null;
                    Cell.WarehouseCell = null;
                }
                else
                {
                    Location.Location = _selectedLocation;
                    SelectedCell = _selectedLocation.Cell;
                    Cell.WarehouseCell = _selectedLocation.Cell;
                }

                IsLoading = true;
                NotifyOfPropertyChange(() => SelectedLocation);
                IsLoading = false;
            }
        }

        public bool HasPermission { get { return Global.Instance.CheckUserPerm(); } }

        public IEnumerable<Orientation> OrientationTypes { get { return Enum.GetValues(typeof(Orientation)).Cast<Orientation>(); } }

        public Orientation WarehouseOrientation
        {
            get { return CellOrientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical; }
        }

        public Orientation CellOrientation
        {
            get { return _CellOrientation; }
            set
            {
                _CellOrientation = value;
                NotifyOfPropertyChange(() => CellOrientation);

                NotifyOfPropertyChange(() => WarehouseOrientation);
                NotifyOfPropertyChange(() => WarehouseVertical);
                NotifyOfPropertyChange(() => WarehouseHorizontal);

                Task.Factory.StartNew(() =>
                {
                    CellsCollectionRack1.ToList().ForEach(x => x.CellOrientation = CellOrientation);
                    CellsCollectionRack2.ToList().ForEach(x => x.CellOrientation = CellOrientation);
                });
            }
        }

        public EAisleDirection AisleDirection { get; protected set; }

        public bool WarehouseVertical { get { return CellOrientation == Orientation.Vertical; } }

        public bool WarehouseHorizontal { get { return CellOrientation == Orientation.Horizontal; } }

        public int CurrentAisle
        {
            get { return _currentAisle; }
            set
            {
                _currentAisle = value;
                NotifyOfPropertyChange(() => CurrentAisle);
            }
        }

        public int CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                _currentLevel = value;
                NotifyOfPropertyChange(() => CurrentLevel);
            }
        }

        public string CurrentAisleName
        {
            get { return _currentAisleName; }
            set
            {
                _currentAisleName = value;
                NotifyOfPropertyChange(() => CurrentAisleName);
            }
        }

        public bool CanRequestMap { get; private set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public LevelViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            Filter = new FilterViewModel(_windowManager, _eventAggregator);
            Filter.ConductWith(this);

            Cell = new WarehouseCellViewModel(_windowManager, _eventAggregator);
            Location = new WhsCellsLocationViewModel(_windowManager, _eventAggregator);

            CellOrientation = (Orientation)Enum.Parse(typeof(Orientation), ConfigurationManager.AppSettings["CellOrientation"]);
            AisleDirection = (EAisleDirection)Enum.Parse(typeof(EAisleDirection), ConfigurationManager.AppSettings["AisleDirection"]);
            _compactableCells = null;
            UserPerm = Global.Instance.CheckUserPerm();

            BindingOperations.EnableCollectionSynchronization(CellsCollectionRack1, _lockObj);
            BindingOperations.EnableCollectionSynchronization(CellsCollectionRack2, _lockObj);


        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Common.Aisle = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].Aisles.FirstOrDefault().Key;
            //Common.Aisle = 1;
            Common.Level = 1;
            Common.Rack = 1;
            Common.ZoomFactor = 6;
            SelectedLocation = null;
            CanRequestMap = false; // Common.StcController > 0;

            await RefreshAllAsync();

            Aisle = new AisleViewModel(_windowManager, _eventAggregator, Common.Warehouse, Common.Aisle);
            Floor = new FloorViewModel(_windowManager, _eventAggregator, Common.Warehouse, Common.Aisle, Common.Level);
            Bays = new BayViewModel(_windowManager, _eventAggregator, Common.Warehouse, Common.Aisle);
            //Rack = new FrontRackViewModel(_windowManager, _eventAggregator, Common.Warehouse, Common.Aisle, Common.Rack);
            //Rack.ConductWith(this);

            AgilogDll.WarehouseCellMgr.Ptr.OnSelectionChanged += Warehouse_OnSelectionChanged;

            await base.OnInitializeAsync(cancellationToken);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            AgilogDll.WarehouseCellMgr.Ptr.OnSelectionChanged -= Warehouse_OnSelectionChanged;

            await base.OnDeactivateAsync(close, cancellationToken);
        }
       
        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            _eventAggregator.PublishOnUIThreadAsync(false);
        }

        #endregion

        #region Public methods

        public async Task RefreshAllAsync()
        {
            IsLoading = true;

            await Task.Factory.StartNew(() =>
            {
                CellsCollectionRack1.Clear();
                CellsCollectionRack2.Clear();

                UpdateRefreshArea();

                CurrentAisle = Common.Aisle;
                CurrentLevel = Common.Level;
                CurrentAisleName = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].Aisles[CurrentAisle].ASL_Desc.TrimUI();       
                
                var cells = AgilogDll.WarehouseCellMgr.Ptr.GetCells(Common.Warehouse, CurrentAisle, CurrentLevel);
                if (CellOrientation == Orientation.Horizontal)
                {
                    if (AisleDirection == EAisleDirection.Left2Right)
                        cells = cells.OrderByDescending(c => c.X).ToList();
                    else
                        cells = cells.OrderBy(c => c.X).ToList();
                }
                else
                {
                    if (AisleDirection == EAisleDirection.Left2Right)
                        cells = cells.OrderBy(c => c.X).ToList();
                    else
                        cells = cells.OrderByDescending(c => c.X).ToList();
                }

                cells.ForEach(c =>
                {
                    c.CellOrientation = CellOrientation;
                    c.SetViewZoom(Common.ZoomFactor / 5.0);
                });
                //---------------------------------------------------------------------------------------------------------------
                // Locko le liste perchè l'evento di filtro viene lanciato n volte e non voglio che si pestino i piedi a vicenda
                //---------------------------------------------------------------------------------------------------------------
                lock (CellsCollectionRack1)
                {
                    CellsCollectionRack1.Clear();
                    cells.Where(c => c.RCK_Side == "L" && AisleDirection == EAisleDirection.Left2Right ||
                                     c.RCK_Side == "R" && AisleDirection == EAisleDirection.Right2Left).ToList().ForEach(x => CellsCollectionRack1.Add(x));
                }
                lock (CellsCollectionRack2)
                {
                    CellsCollectionRack2.Clear();
                    cells.Where(c => c.RCK_Side == "R" && AisleDirection == EAisleDirection.Left2Right ||
                                     c.RCK_Side == "L" && AisleDirection == EAisleDirection.Right2Left).ToList().ForEach(x => CellsCollectionRack2.Add(x));
                }
                if (SelectedCell != null)
                {
                    SelectedCell.IsSelected = true;
                }

                if (_compactableCells == null || Common.AisleChanged)
                {
                    if (_compactableCells != null)
                    {
                        lock (_compactableCells)
                        {
                            _compactableCells = CompactableCell.GetList((SqlConnection)Global.Instance.ConnGlobal, Common.Warehouse, Common.Aisle);
                            _compactableIndex = -1;
                            _compactableCell = null;
                        }
                    }
                    else
                    {
                        _compactableCells = CompactableCell.GetList((SqlConnection)Global.Instance.ConnGlobal, Common.Warehouse, Common.Aisle);
                        _compactableIndex = -1;
                        _compactableCell = null;
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SelectCompactableCell();
                });

                Common.AisleChanged = false;
                Common.LevelChanged = false;
            });

            IsLoading = false;
        }

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            if (message == "CELL")
            {
                //SelectedCell = Rack.SelectedCell;

                if (SelectedLocation != null &&
                    SelectedLocation.Cell != SelectedCell)
                {
                    SelectedLocation.IsSelected = false;
                    SelectedLocation = null;
                }
                //if (SelectedCell.Y != Common.Level)
                //{
                //    Common.Level = SelectedCell.Y;
                //    Filter.Level = SelectedCell.Y;
                //    Floor.CurrentLevel = Common.Level;
                //}
            }

            var parts = message.Split('=');
            if (parts.Length == 2)
            {
                switch (parts[0])
                {
                    case "AISLE":
                        Common.Aisle = int.Parse(parts[1]);
                        Aisle.AisleNum = Common.Aisle;
                        Bays.AisleNum = Common.Aisle;
                        //Rack.AisleNum = Common.Aisle;
                        break;

                    case "LEVEL":
                        Common.Level = int.Parse(parts[1]);
                        Floor.Aisle = Common.Aisle;
                        Floor.CurrentLevel = Common.Level;
                        break;

                    //case "RACK":
                    //    Common.Rack = int.Parse(parts[1]);
                    //    Rack.RackNum = Common.Rack;
                    //    break;

                    case "ZOOM":
                        Common.ZoomFactor = int.Parse(parts[1]);
                        break;
                }
            }

            if (Common.AisleChanged || Common.LevelChanged)
            {
                Common.AisleChanged = false;
                Common.LevelChanged = false;

                AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();
                SelectedLocation = null;
                _compactableIndex = -1;
                _compactableCell = null;

                await RefreshAllAsync();
            }
            else if (Common.ZoomFactorChanged)
            {
                var cells = AgilogDll.WarehouseCellMgr.Ptr.GetCells(Common.Warehouse, CurrentAisle, CurrentLevel);
                cells.ForEach(c => c.SetViewZoom(Common.ZoomFactor / 5.0));
            }
        }

       

        public async Task RecompactAllAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to launch the operation for the selected aisle?")))
                return;

            IsLoading = true;

            var row = await _utils.Cells_RecompactAsync(Common.Warehouse, Common.Aisle);

            IsLoading = false;

            string error = row.GetValue(1);
            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, SelectedLocation.LastError);
                return;
            }

            SnackBarMessage = Global.Instance.LangTl("Operation launched successfully");
        }

        public async Task RequestMapAsync()
        {

        }

        #endregion

        #region Private methods

        private void UpdateRefreshArea()
        {
            AgilogDll.WarehouseCellMgr.Ptr.SetRefreshArea(
                Common.Warehouse,
                Common.Aisle,
                null,
                Common.Level);

            DisplayName = $"{Global.Instance.LangTl("Aisle")} {Common.Aisle} - " +
                          $"{Global.Instance.LangTl("Floor")} {Common.Level}";
        }

        private void SelectCompactableCell()
        {
            if (_compactableCell == null) return;

            AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();

            if (_compactableCell.Warehouse == Common.Warehouse &&
                _compactableCell.Aisle == Common.Aisle &&
                _compactableCell.CEL_Y == Common.Level)
            {
                var warehouseCell = CellsCollectionRack1.Union(CellsCollectionRack2).FirstOrDefault(c => c.CEL_Id == _compactableCell.CEL_Id);
                if (warehouseCell == null) return;

                warehouseCell.IsSelected = true;
                SelectedCell = warehouseCell;
                warehouseCell.Locations.First().IsSelected = true;
                SelectedLocation = warehouseCell.Locations.First();
            }
            else
            {
                Common.Aisle = _compactableCell.Aisle;
                Common.Level = _compactableCell.CEL_Y;

                AgilogDll.WarehouseCellMgr.Ptr.DeselectAll();
                SelectedLocation = null;

                RefreshAllAsync();
            }
        }

        private async Task Recompact(WarehouseCell cell = null)
        {

        }

        #endregion

        #region Events

        private void Warehouse_OnSelectionChanged(object sender, GenericEventArgs e)
        {
            AgilogDll.WhsCellsLocation location = e.Argument as AgilogDll.WhsCellsLocation;
            if (location == null) return;

            if (location.IsSelected) SelectedLocation = location.Clone() as AgilogDll.WhsCellsLocation;
            else SelectedLocation = null;
        }

        #endregion
    }
}
