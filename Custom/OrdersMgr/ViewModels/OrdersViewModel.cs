using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Threading;
using System.Reflection.Metadata.Ecma335;
//using AgilogDll;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(OrdersViewModel))]
    class OrdersViewModel : Conductor<Screen>, IHandle<string>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private Dictionary<string, object> _windowSettings;
        private AgilogDll.RunUtils _utils;//Aggiungi Agilog prima di RunUtils

        /// <summary>
        /// Numero della baia
        /// </summary>
        private readonly int? BayNr;
        private AgilogDll.MisOrder _SelectedOrder = null;
        private bool _IsLoading = false;
        private bool _FilterToggleButtonIsChecked;
        private string _SnackBarMessage;
        private object _lockObj = new object();
        private List<HndBayBay> _bays;

        private bool _selectAll;
        private List<AgilogDll.MisOrder> _SelectedOrders = null;

        #endregion

        #region Properties

        private FilterOrderViewModel _FilterOrder;

        public FilterOrderViewModel FilterOrder
        {
            get { return _FilterOrder; }
            private set
            {
                _FilterOrder = value;
                NotifyOfPropertyChange(() => FilterOrder);
            }
        }

        public ObservableCollection<AgilogDll.MisOrder> OrdersList { get; set; } = new ObservableCollection<AgilogDll.MisOrder>();

        public AgilogDll.MisOrder SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                _SelectedOrder = value;

                if (_SelectedOrder != null)
                {
                    SelectAll = false;
                    _SelectedOrder.IsSelected = true;
                }

                NotifyOfPropertyChange(() => SelectedOrder);
                RefreshSelectedOrders();

                RefreshRowsAsync();
            }
        }

        public ObservableCollection<AgilogDll.MisOrdersDetail> OrdersLinesList { get; set; } = new ObservableCollection<AgilogDll.MisOrdersDetail>();

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanEdit);

                if (value) _eventAggregator.PublishOnUIThreadAsync(EUserOperations.Start);
                else _eventAggregator.PublishOnUIThreadAsync(EUserOperations.Noop);
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

        public bool CanStart
        {
            get
            {
                return _SelectedOrders.Count > 0 &&
                       !_SelectedOrders.Any(o => o.OrderPhaseType != OrdPhaseType.VAL || o.ORD_Fixed);
            }
        }

        public bool CanPrepare
        {
            get
            {
                return CanStart;
            }
        }

        public bool CanStop
        {
            get
            {
                return _SelectedOrders.Count > 0 &&
                       !_SelectedOrders.Any(o => o.OrderPhaseType != OrdPhaseType.SCH);
            }
        }

        public bool CanExport
        {
            get
            {
                return _SelectedOrders.Count > 0 &&
                       !_SelectedOrders.Any(o => o.ORD_Fixed) &&
                       !_SelectedOrders.Any(o => o.ORD_EXT_Code.Equals("COMPL"));
            }
        }

        public bool CanViewUDC
        {
            get
            {
                return _SelectedOrders.Count == 1;
            }
        }

        public bool CanEdit
        {
            get
            {
                return _SelectedOrders.Count == 1 &&
                       SelectedOrder != null &&
                       !SelectedOrder.ORD_Fixed &&
                       !SelectedOrder.ORD_Auto &&
                       SelectedOrder.OrderPhaseType == OrdPhaseType.VAL &&
                       string.IsNullOrWhiteSpace(SelectedOrder.ORD_EXT_Code) &&
                       SelectedOrder.OrderSchedType == OrdSchedType.M &&
                       !IsLoading;
            }
        }

        public bool CanDelete
        {
            get
            {
                return _SelectedOrders.Count == 1 &&
                       SelectedOrder != null &&
                       !SelectedOrder.ORD_Fixed &&
                       !SelectedOrder.ORD_Auto &&
                       SelectedOrder.OrderPhaseType == OrdPhaseType.VAL &&
                       string.IsNullOrWhiteSpace(SelectedOrder.ORD_EXT_Code) &&
                       SelectedOrder.OrderSchedType == OrdSchedType.M;
            }
        }

        public ICommand ErrorCommand { get; }

        public bool SelectAll
        {
            get { return _selectAll; }
            set
            {
                _selectAll = value;
                NotifyOfPropertyChange(() => SelectAll);

                OrdersList.ToList().ForEach(o => o.IsSelected = _selectAll);

                RefreshSelectedOrders();
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public OrdersViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, int? bayNr = null)
        {
            DisplayName = Global.Instance.LangTl("Order list");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));
            ErrorCommand = new CustomCommandImplementation(ViewErrorsAsync);
            _SelectedOrders = new List<AgilogDll.MisOrder>();

            BayNr = bayNr;

            FilterOrder = new FilterOrderViewModel(_windowManager, _eventAggregator);
            FilterOrder.ConductWith(this);

            _windowSettings = new Dictionary<string, object>()
            {
                { "BorderThickness", new Thickness(1) },
                { "WindowState", WindowState.Maximized },
                //{ "ResizeMode", ResizeMode.NoResize },
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                { "MinHeight", 600 },
                { "MinWidth", 800 },
                { "SizeToContent", SizeToContent.Manual },
                { "Icon", Global.Instance.GetAppIcon() }
            };

            BindingOperations.EnableCollectionSynchronization(OrdersList, _lockObj);
            BindingOperations.EnableCollectionSynchronization(OrdersLinesList, _lockObj);
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            ParamManager.Init(Global.Instance.ConnGlobal);

            await base.OnInitializeAsync(cancellationToken);
        }

        protected override async void OnViewLoaded(object view)
        {
            IsLoading = true;

            await MasterDataManager.Instance.LoadCustomersList();

            await RefreshAllAsync();

            base.OnViewLoaded(view);
        }

        #endregion

        #region Public methods

        public async Task StopAllAsync()
        {
            if (await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to stop ALL the orders?")))
            {
                IsLoading = true;

                var ret = await _utils.StopAllOrdersAsync();

                IsLoading = false;

                if (!string.IsNullOrWhiteSpace(ret))
                    await Global.ErrorAsync(_windowManager, ret);
                else
                {
                    SnackBarMessage = Global.Instance.LangTl("General abort command has been successfully sent");
                }
                await RefreshAllAsync();
            }
        }

        public async Task StartAsync()
        {
            if (_SelectedOrders.Count <= 0 ||
                !await Global.ConfirmAsync(_windowManager, "Vuoi davvero eseguire il lancio COMPLETO degli ordini selezionati?")) return;

            HndBayBay bay = null;

            foreach (var order in _SelectedOrders)
            {
                // La baia destinazione dipende dal tipo di ordine
                if (order.OrderType == OrdType.P) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "PK");
                else if (order.OrderType == OrdType.S) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "OUT");

                if (bay == null || !bay.BAY_Enabled)
                {
                    await Global.AlertAsync(_windowManager, $"Nessuna baia abilitata per l'esecuzione dell'ordine {order.ORD_OrderCode}");
                    return;
                }

                IsLoading = true;

                string error = null;
                bool retVal = await Task.Run(() =>
                {
                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    var ret = _utils.RunOrderCustom((SqlConnection)conn, order, Global.Instance.CurrentUser.Code, ref error, bay.BAY_Num);
                    return ret;
                });

                IsLoading = false;

                if (!retVal)
                {
                    await Global.AlertAsync(_windowManager, $"Si è verificato un errore nel lancio dell'ordine {order.ORD_OrderCode}: {error}");
                    return;
                }
            }

            await RefreshAllAsync();
        }

        public async Task StartPickAsync()
        {
            if (_SelectedOrders.Count <= 0 ||
                !await Global.ConfirmAsync(_windowManager, "Vuoi davvero lanciare solo il PICKING per gli ordini selezionati?")) return;

            HndBayBay bay = null;

            foreach (var order in _SelectedOrders)
            {
                // La baia destinazione dipende dal tipo di ordine
                if (order.OrderType == OrdType.P) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "PK");
                else if (order.OrderType == OrdType.S) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "OUT");

                if (bay == null || !bay.BAY_Enabled)
                {
                    await Global.AlertAsync(_windowManager, $"Nessuna baia abilitata per l'esecuzione dell'ordine {order.ORD_OrderCode}");
                    return;
                }

                IsLoading = true;

                string error = null;
                bool retVal =
                await Task.Run(() =>
                {
                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    var ret = _utils.RunOrderCustom((SqlConnection)conn, order, Global.Instance.CurrentUser.Code, ref error, bay.BAY_Num, true);
                    return ret;
                });

                IsLoading = false;

                if (!retVal)
                {
                    await Global.AlertAsync(_windowManager, $"Si è verificato un errore nel lancio dell'ordine {order.ORD_OrderCode}: {error}");
                    return;
                }
            }

            await RefreshAllAsync();
        }

        public async Task PrepareAsync()
        {
            if (_SelectedOrders.Count <= 0 ||
                !await Global.ConfirmAsync(_windowManager, "Vuoi davvero eseguire la PREPARAZIONE degli ordini selezionati?")) return;

            foreach (var order in _SelectedOrders)
            {
                // Verifico se l'ordine è già stato preparato
                var prepared = await Task.Run(() =>
                {
                    var query = $@"
                        SELECT COUNT([RES_ID])
                        FROM [UDC_RESERVATIONS]
                        WHERE [RES_ODT_ORD_OrderCode] = {order.ORD_OrderCode.SqlFormat()}";

                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    var prep = DbUtils.ExecuteScalar(query, conn);

                    if (prep != null) return ((int?)prep).Value > 0;
                    return false;
                });

                if (prepared &&
                    !await Global.ConfirmAsync(_windowManager, $"L'ordine {order.ORD_OrderCode} è già stato preparato. Vuoi ripetere l'operazione?")) continue;

                HndBayBay bay = null;

                // La baia destinazione dipende dal tipo di ordine
                if (order.OrderType == OrdType.P) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "PK");
                else if (order.OrderType == OrdType.S) bay = _bays.FirstOrDefault(b => b.BAY_BYT_Code == "OUT");

                if (bay == null || !bay.BAY_Enabled)
                {
                    await Global.AlertAsync(_windowManager, $"Nessuna baia abilitata per l'esecuzione dell'ordine {order.ORD_OrderCode}");
                    return;
                }

                IsLoading = true;

                string error = null;
                bool retVal = await Task.Run(() =>
                {
                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    var ret = _utils.RunOrderCustom((SqlConnection)conn, order, Global.Instance.CurrentUser.Code, ref error, bay.BAY_Num, false, true);
                    return ret;
                });

                IsLoading = false;

                if (!retVal)
                {
                    await Global.AlertAsync(_windowManager, $"Si è verificato un errore nel lancio dell'ordine {order.ORD_OrderCode}: {error}");
                    return;
                }
            }

            await RefreshAllAsync();
        }

        public async Task StopAsync()
        {
            if (_SelectedOrders.Count <= 0 ||
                !await Global.ConfirmAsync(_windowManager, "Vuoi davvero eseguire lo STOP degli ordini selezionati?")) return;

            foreach (var order in _SelectedOrders)
            {
                IsLoading = true;

                string error = null;
                bool retVal = await Task.Run(() =>
                {
                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    var ret = _utils.StopOrderCustom((SqlConnection)conn, SelectedOrder, Global.Instance.CurrentUser.Code, ref error);
                    return ret;
                });

                IsLoading = false;

                if (!retVal)
                {
                    await Global.AlertAsync(_windowManager, $"Si è verificato un errore nel tentativo di STOP dell'ordine {order.ORD_OrderCode}: {error}");
                    return;
                }
            }

            SnackBarMessage = "Richiesta di STOP inviata con successo";
            await RefreshAllAsync();
        }

        public async Task ExportAsync()
        {

        }

        public void New()
        {
            
        }

        public async void Edit()
        {
            
        }

        public async Task DeleteAsync()
        {
            var order = _SelectedOrders.FirstOrDefault();

            if (order != null &&
                await Global.ConfirmAsync(_windowManager, string.Format(Global.Instance.LangTl("Do you want to delete the order {0}?"), order.ORD_OrderCode)))
            {
                if (!order.Delete())
                {
                    await Global.ErrorAsync(_windowManager, order.LastError);
                    return;
                }

                SnackBarMessage = Global.Instance.LangTl("Order has been deleted");

                await RefreshAllAsync();
            }
        }

        public async Task ViewUDCAsync()
        {
            var order = _SelectedOrders.FirstOrDefault();

            if (order == null) return;

            var vm = new BookedUDCsViewModel(_windowManager, _eventAggregator, order);

            await _windowManager.ShowWindowAsync(vm);
        }

        public async void ViewErrorsAsync(object args)
        {
            var vm = new OrderDetErrorsViewModel(_windowManager, _eventAggregator, (AgilogDll.MisOrdersDetail)args);

            await _windowManager.ShowWindowAsync(vm);
        }

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            switch (message)
            {
                case "APPLY_FILTER":
                case "CLEAR_FILTER":
                    await RefreshAllAsync();
                    break;

                default:
                    break;
            }
        }

        public async Task RefreshRowsAsync()
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                OrdersLinesList.Clear();

                if (_SelectedOrder != null)
                {
                    _SelectedOrder.LoadLines();
                    _SelectedOrder.Lines.ForEach(x => OrdersLinesList.Add(x));
                }
            });

            IsLoading = false;
        }

        public async Task RefreshAllAsync()
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                _bays?.Clear();
                _bays = HndBayBay.GetList<HndBayBay>(Global.Instance.ConnGlobal);

                SelectedOrder = null;
                OrdersList.Clear();

                var result = GetAllOrders();

                result.OrderBy(x => x.ORD_Fixed ? 999 : (x.ORD_MOP_Code == "VAL" ? 0 : (x.ORD_MOP_Code == "SCH" ? 1 : 2))).
                       ThenBy(x => x.ORD_OrderCode).ToList().ForEach(x => OrdersList.Add(x));
            });

            IsLoading = false;
        }

        public void RefreshSelectedOrders()
        {
            _SelectedOrders.Clear();
            _SelectedOrders.AddRange(OrdersList.Where(o => o.IsSelected));

            NotifyOfPropertyChange(() => CanStart);
            NotifyOfPropertyChange(() => CanPrepare);
            NotifyOfPropertyChange(() => CanStop);
            NotifyOfPropertyChange(() => CanExport);
            NotifyOfPropertyChange(() => CanEdit);
            NotifyOfPropertyChange(() => CanDelete);
            NotifyOfPropertyChange(() => CanViewUDC);
        }

        #endregion

        #region Private methods

        private List<AgilogDll.MisOrder> GetAllOrders()
        {
            var result = MisOrder.GetList<AgilogDll.MisOrder>(DbUtils.CloneConnection(Global.Instance.ConnGlobal)).Where(o => !o.ORD_Fixed || o.ORD_Visible);

            if (!string.IsNullOrWhiteSpace(FilterOrder.OrderCode))
                result = result.Where(o => o.ORD_OrderCode.Contains(FilterOrder.OrderCode));

            if (!string.IsNullOrWhiteSpace(FilterOrder.OrderDescription))
                result = result.Where(o => o.ORD_Description.Contains(FilterOrder.OrderDescription));

            if (FilterOrder.FromCreationDate.HasValue)
                result = result.Where(o => o.ORD_DocDate >= FilterOrder.FromCreationDate.Value);

            if (FilterOrder.ToCreationDate.HasValue)
                result = result.Where(o => o.ORD_DocDate <= FilterOrder.ToCreationDate.Value);

            if (FilterOrder.FromDueDate.HasValue)
                result = result.Where(o => o.ORD_DueDate >= FilterOrder.FromDueDate.Value);

            if (FilterOrder.ToDueDate.HasValue)
                result = result.Where(o => o.ORD_DueDate <= FilterOrder.ToDueDate.Value);

            if (!string.IsNullOrWhiteSpace(FilterOrder.MissionType))
                result = result.Where(o => o.ORD_MIT_Code.Equals(FilterOrder.MissionType));

            if (FilterOrder.Priority.HasValue)
                result = result.Where(o => o.ORD_Priority == FilterOrder.Priority.Value);

            if (!string.IsNullOrWhiteSpace(FilterOrder.Phase))
                result = result.Where(o => o.ORD_MOP_Code.Equals(FilterOrder.Phase));

            return result.ToList();
        }


        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {

        }

        #endregion
    }
}
