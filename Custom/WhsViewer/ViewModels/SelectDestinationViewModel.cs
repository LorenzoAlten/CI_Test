using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace WhsViewer.ViewModels
{
    [Export(typeof(SelectDestinationViewModel))]
    public class SelectDestinationViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private object _lockObj = new object();

        private bool _IsLoading = false;

        private AgilogDll.WhsCellsLocation _sourceCel;
        private int _aisle;
        private int _level;
        private long _channel;

        private List<CustomComboBoxItem> _aisles;
        private List<CustomComboBoxItem> _levels;
        private List<CustomComboBoxItem> _channels;

        private List<ValidCell> _validCells;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public int Aisle
        {
            get { return _aisle; }
            set
            {
                _aisle = value;
                LoadLevels();

                NotifyOfPropertyChange(() => Aisle);
                NotifyOfPropertyChange(() => CanConfirm);
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                LoadChannels();

                NotifyOfPropertyChange(() => Level);
                NotifyOfPropertyChange(() => CanConfirm);
            }
        }

        public long Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                NotifyOfPropertyChange(() => Channel);
                NotifyOfPropertyChange(() => CanConfirm);
            }
        }

        public List<CustomComboBoxItem> Aisles
        {
            get { return _aisles; }
            set
            {
                _aisles = value;
                NotifyOfPropertyChange(() => Aisles);
            }
        }

        public List<CustomComboBoxItem> Levels
        {
            get { return _levels; }
            set
            {
                _levels = value;
                NotifyOfPropertyChange(() => Levels);
            }
        }

        public List<CustomComboBoxItem> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                NotifyOfPropertyChange(() => Channels);
            }
        }

        public bool CanConfirm
        {
            get
            {
                return Aisle > 0 &&
                       Level > 0 &&
                       Channel > 0;
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public SelectDestinationViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgilogDll.WhsCellsLocation sourceCelId)
        {
            DisplayName = string.Empty;

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _sourceCel = sourceCelId;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            _validCells = ValidCell.GetValidCells((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal), _sourceCel);

            Aisles = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
                                          Aisles.Values.Where(a => a.ASL_Num == _sourceCel.Cell.CEL_RCK_ASL_Num).
                                          Select(a => new CustomComboBoxItem(a.ASL_Desc, a.ASL_Num)).ToList();
            NotifyOfPropertyChange(() => Aisles);
            Aisle = (int)Aisles.FirstOrDefault(a => (int)a.Value == Common.Aisle).Value;

            await base.OnInitializeAsync(cancellationToken);
        }


      
        #endregion

        #region Public Methods

        public void Confirm()
        {
            if (_sourceCel.LOC_CEL_Id == _channel)
            {
                Global.AlertAsync(_windowManager, Global.Instance.LangTl("Source and destination must be different channels"));
                return;
            }

            TryCloseAsync(true);
        }

        public void Cancel()
        {
            _aisle = -1;
            _level = -1;
            _channel = -1;

            TryCloseAsync(false);
        }

        #endregion

        #region Private methods

        private void LoadLevels()
        {
            if (_validCells != null)
            {
                Levels = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
                              Aisles[Aisle].
                              Cells.Values.Where(c => _validCells.Any(v => v.Y == c.Y)).
                              Select(c => c.Y).Distinct().
                              Select(l => new CustomComboBoxItem($"{Global.Instance.LangTl("Floor")} {l}", l)).ToList();
            }
            else
            {
            Levels = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
                                          Aisles[Aisle].
                                          Cells.Values.
                                          Select(c => c.Y).Distinct().
                                          Select(l => new CustomComboBoxItem($"{Global.Instance.LangTl("Floor")} {l}", l)).ToList();
            }
            NotifyOfPropertyChange(() => Levels);
            Level = (int)Levels.FirstOrDefault(a => (int)a.Value == Common.Level).Value;
        }

        private void LoadChannels()
        {
            if (_validCells != null)
            {
            Channels = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
                                            Aisles[Aisle].
                              Cells.Values.Where(c => c.Y == Level && _validCells.Any(v => v.Id == c.CEL_Id)).
                                            Select(c => new CustomComboBoxItem($"{Global.Instance.LangTl("Cell")} {c.CEL_Id} - X={c.X} - {c.RCK_Desc}", c.CEL_Id)).ToList();
            }
            else
            {
                Channels = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
                                            Aisles[Aisle].
                                            Cells.Values.Where(c => c.Y == Level && c.CEL_HGT_Num >= _sourceCel.Udc.UDC_HGT_Num &&
                                                                    c.Locations.Any(l => l.LOC_UDT_Code == _sourceCel.Udc.UDC_UDT_Code &&
                                                                                         l.LOC_EnabledIN && !l.LOC_Error && l.LOC_LOS_Code == "L")).Distinct().
                                            Select(c => new CustomComboBoxItem($"{Global.Instance.LangTl("Cell")} {c.CEL_Id} - X={c.X} - {c.RCK_Desc}", c.CEL_Id)).ToList();
            }
            NotifyOfPropertyChange(() => Channels);

            if (Channels.Count == 0)
            {
                Global.AlertAsync(_windowManager, Global.Instance.LangTl("No suitable channel found"));
                return;
            }

            Channel = (long)Channels.FirstOrDefault().Value;
        }

        #endregion
    }

    public class ValidCell
    {
        #region Properties

        public int Y { get; private set; }
        public int Id { get; private set; }

        #endregion

        #region Constructor

        public ValidCell() {}

        public ValidCell(DataRow row)
        {
            LoadFromRow(row);
        }

        #endregion

        #region Private Methods

        private void LoadFromRow(DataRow row)
        {
            Y = row.GetValueI("CEL_Y");
            Id = row.GetValueI("CEL_Id");
        }

        #endregion

        #region Static Methods

        public static List<ValidCell> GetValidCells(SqlConnection conn, AgilogDll.WhsCellsLocation sourceCel)
        {
            string query = string.Empty;
            List<ValidCell> validCells = new List<ValidCell>();

            query = $@" SELECT DISTINCT [CEL_Y], [CEL_Id]
                        FROM [WHS_CELLS_RACKS]
                        JOIN [WHS_CELLS]
                            ON [CRK_CEL_Id] = [CEL_Id]
                        JOIN [WHS_CELLS_LOCATIONS]
                            ON [CEL_Id] = [LOC_CEL_Id]
                        WHERE [CRK_RCK_ASL_WHS_Code] = {Common.Warehouse.SqlFormat()}
                        AND [CRK_RCK_ASL_Num] = {sourceCel.Cell.CEL_RCK_ASL_Num.SqlFormat()}
                        AND [CEL_Id] <> {sourceCel.LOC_CEL_Id.SqlFormat()}
                        AND [CEL_HGT_Num] >= {sourceCel.Udc.UDC_HGT_Num.SqlFormat()}
                        AND [CEL_WGH_Num] >= {sourceCel.Udc.UDC_WGH_Num.SqlFormat()}
                        AND [LOC_UDT_Code] = {sourceCel.Udc.UDC_UDT_Code.SqlFormat()}
                        AND [LOC_EnabledIN] = 1
                        AND [LOC_Error] = 0
                        AND [LOC_LOS_Code] = 'L'
                        ORDER BY [CEL_Y], [CEL_Id]";

            DataTable dt = DbUtils.ExecuteDataTable(query, conn);

            if (dt == null) return null;

            foreach (DataRow row in dt.Rows)
            {
                validCells.Add(new ValidCell(row));
            }

            return validCells;
        }

        #endregion
    }
}
