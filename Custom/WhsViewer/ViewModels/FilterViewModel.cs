using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WhsViewer.ViewModels
{
    [Export(typeof(FilterViewModel))]
    class FilterViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private int _aisle;
        private int _level;
        private int _rack;
        private int _zoomFactor;
        private List<GenericFilterItem> _aisles;
        private List<GenericFilterItem> _levels;
        private List<GenericFilterItem> _racks;

        #endregion

        #region Properties

        public int Aisle
        {
            get { return _aisle; }
            set
            {
                _aisle = value;
                NotifyOfPropertyChange(() => Aisle);

                LoadRacksAsync();
                LoadLevelsAsync();
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                NotifyOfPropertyChange(() => Level);
            }
        }

        public int Rack
        {
            get { return _rack; }
            set
            {
                _rack = value;
                NotifyOfPropertyChange(() => Rack);
            }
        }

        public int ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                _zoomFactor = value;
                NotifyOfPropertyChange(() => ZoomFactor);
            }
        }

        public List<GenericFilterItem> Aisles
        {
            get { return _aisles; }
            set
            {
                _aisles = value;
                NotifyOfPropertyChange(() => Aisles);
            }
        }

        public List<GenericFilterItem> Levels
        {
            get { return _levels; }
            set
            {
                _levels = value;
                NotifyOfPropertyChange(() => Levels);
            }
        }

        public List<GenericFilterItem> Racks
        {
            get { return _racks; }
            set
            {
                _racks = value;
                NotifyOfPropertyChange(() => Racks);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FilterViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Aisles = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse].
               Aisles.Values.Select(a => new GenericFilterItem(a.ASL_Num, a.ASL_Desc)).ToList();

            Aisle = Aisles.FirstOrDefault(a => a.Value == Common.Aisle).Value;

            Racks = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse]
                .Aisles.Values.FirstOrDefault(a => a.ASL_Num == Common.Aisle)
                .Racks.Values.Select(a => new GenericFilterItem(a.RCK_Num, a.RCK_Desc)).ToList();

            Rack = Racks.FirstOrDefault().Value;

            //LoadLevelsAsync();

            ZoomFactor = (int)Common.ZoomFactor;

            await base.OnInitializeAsync(cancellationToken);
        }        
        #endregion

        private async Task LoadLevelsAsync()
        {
            await Task.Factory.StartNew(() => 
            {
                Levels = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse]
                                             .Aisles[Aisle]
                                             .Cells.Values
                                             .Select(c => c.Y).Distinct()
                                             .Select(l => new GenericFilterItem(l, $"{Global.Instance.LangTl("Floor")} {l}"))
                                             .OrderByDescending(x => x.Value).ToList();
                Level = Levels.FirstOrDefault(l => l.Value == Common.Level).Value; 
            });
        }

        private async Task LoadRacksAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                Racks = AgilogDll.WarehouseCellMgr.Ptr.Warehouses[Common.Warehouse]
                                        .Aisles[Aisle]
                                        .Racks.Values
                                        .Select(a => new GenericFilterItem(a.RCK_Num, a.RCK_Desc)).ToList();

                Rack = Racks.FirstOrDefault().Value;
            });
        }

        #region Public methods

        public void Apply()
        {
            _eventAggregator.PublishOnUIThreadAsync($"{Aisle};{Level};{ZoomFactor}");
        }

        public void ChangeAisle()
        {
            _eventAggregator.PublishOnUIThreadAsync($"AISLE={_aisle}");
        }

        public void ChangeLevel()
        {
            _eventAggregator.PublishOnUIThreadAsync($"LEVEL={_level}");
        }

        public void ChangeRack()
        {
            _eventAggregator.PublishOnUIThreadAsync($"RACK={_rack}");
        }

        public void ChangeZoom()
        {
            _eventAggregator.PublishOnUIThreadAsync($"ZOOM={_zoomFactor}");
        }

        #endregion
    }

    public class GenericFilterItem
    {
        public int Value { get; set; }
        public string Desc { get; set; }

        public GenericFilterItem(int value, string desc)
        {
            Value = value;
            Desc = desc;
        }
    }
}
