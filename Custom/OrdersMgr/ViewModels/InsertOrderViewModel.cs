using mSwAgilogDll;
using Caliburn.Micro;
using Caliburn.Micro.Validation;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(InsertOrderViewModel))]
    class InsertOrderViewModel : ValidatingScreen<InsertOrderViewModel>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private Dictionary<string, object> _windowSettings;

        private MisOrder _Order = null;
        private string _BPA_Desc;
        private MisOrdersDetail _Selected = null;
        private List<MisCfgType> _MisCfgTypeList;
        private Dictionary<object, string> _Dictionary_ItemCodes;
        private Dictionary<object, string> _Dictionary_ItemDescriptions;
        private bool _IsLoading = false;
        private int _NextLineNum;

        #endregion

        #region Properties
         
        /// <summary>
        /// Ordine
        /// </summary>
        public MisOrder Order
        {
            get { return _Order; }
            set
            {
                _Order = value;
                if (_Order.LoadedFromDb)
                {
                    _Order.LoadLines();
                    OrdersDetailList = new ObservableCollection<MisOrdersDetail>(_Order.Lines);
                }
                NotifyOfPropertyChange(() => Order);
                NotifyOfPropertyChange(() => CanNewRow);
                NotifyOfPropertyChange(() => CanEditRow);
                NotifyOfPropertyChange(() => CanDeleteRow);
            }
        }

        /// <summary>
        /// Codice ordine
        /// </summary>
        [Required(ErrorMessage = "Field is required")]
        public string OrderCode
        {
            get { return _Order.ORD_OrderCode; }
            set
            {
                _Order.ORD_OrderCode = value;
                NotifyOfPropertyChange(() => OrderCode);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        /// <summary>
        /// Descrizione ordine
        /// </summary>
        public string Description
        {
            get { return _Order.ORD_Description; }
            set
            {
                _Order.ORD_Description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        /// <summary>
        /// Tipo operazione (Entrata, Inventario, Prelievo, Prelievo per Spedizione, Versamento)
        /// </summary>
        [Required(ErrorMessage = "Field is required")]
        public string MIT_Code
        {
            get { return _Order.ORD_MIT_Code; }
            set
            {
                _Order.ORD_MIT_Code = value;
                NotifyOfPropertyChange(() => MIT_Code);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        /// <summary>
        /// Codice cliente
        /// </summary>
        public string BPA_Code
        {
            get { return _Order.ORD_BPA_Code; }
            set
            {
                _Order.ORD_BPA_Code = value;
                NotifyOfPropertyChange(() => BPA_Code);
            }
        }

        /// <summary>
        /// Descrizione cliente
        /// </summary>
        public string BPA_Desc
        {
            get { return _BPA_Desc; }
            set
            {
                _BPA_Desc = value;
                NotifyOfPropertyChange(() => BPA_Desc);
            }
        }

        /// <summary>
        /// Data fine evasione
        /// </summary>
        [FutureDate]
        public DateTime DueDate
        {
            get { return _Order.ORD_DueDate; }
            set
            {
                _Order.ORD_DueDate = value;
                NotifyOfPropertyChange(() => DueDate);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        /// <summary>
        /// Priorità
        /// </summary>
        public int? Priority
        {
            get { return _Order.ORD_Priority; }
            set
            {
                _Order.ORD_Priority = value;
                NotifyOfPropertyChange(() => Priority);
            }
        }

        /// <summary>
        /// Lista delle righe d'ordine
        /// </summary>
        public ObservableCollection<MisOrdersDetail> OrdersDetailList { get; private set; } = new ObservableCollection<MisOrdersDetail>();

        /// <summary>
        /// Lista tipi operazione
        /// </summary>
        public List<MisCfgType> MisCfgTypeList
        {
            get { return _MisCfgTypeList; }
            set
            {
                _MisCfgTypeList = value;
                NotifyOfPropertyChange(() => MisCfgTypeList);
            }
        }

        /// <summary>
        /// Lista delle priorità accettate
        /// </summary>
        public IEnumerable<int> PriorityList
        {
            get { return Enumerable.Range(0, 11); }
        }

        public MisOrdersDetail Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                NotifyOfPropertyChange(() => Selected);
                NotifyOfPropertyChange(() => CanEditRow);
                NotifyOfPropertyChange(() => CanDeleteRow);
            }
        }

        /// <summary>
        /// Lista dei codici dei clienti
        /// </summary>
        public Dictionary<object, string> Dictionary_BusinessPartnersCodes
        {
            get { return _Dictionary_ItemCodes; }
            set
            {
                _Dictionary_ItemCodes = value;
                NotifyOfPropertyChange(() => Dictionary_BusinessPartnersCodes);
            }
        }

        /// <summary>
        /// Lista delle descrizioni dei clienti
        /// </summary>
        public Dictionary<object, string> Dictionary_BusinessPartnersDescriptions
        {
            get { return _Dictionary_ItemDescriptions; }
            set
            {
                _Dictionary_ItemDescriptions = value;
                NotifyOfPropertyChange(() => Dictionary_BusinessPartnersDescriptions);
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

        #endregion

        #region Constructor

        [ImportingConstructor]
        public InsertOrderViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, MisOrder order = null)
        {
            DisplayName = (order == null) ? Global.Instance.LangTl("New order") : Global.Instance.LangTl("Edit order");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Order = order ?? new MisOrder((SqlConnection)Global.Instance.ConnGlobal);
            _NextLineNum = (OrdersDetailList.Count > 0) ? OrdersDetailList.Max(l => l.ODT_Line) + 1 : 1;

            _windowSettings = new Dictionary<string, object>()
            {
                { "BorderThickness", new Thickness(1) },
                { "WindowState", WindowState.Maximized },
                { "ResizeMode", ResizeMode.NoResize },
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                { "MinHeight", 600 },
                { "MinWidth", 800 },
                { "SizeToContent", SizeToContent.Manual },
                { "Icon", Global.Instance.GetAppIcon() }
            };
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                IsLoading = true;

                if (DictionaryMgr.Instance == null)
                {
                    var d = new DictionaryMgr((SqlConnection)Global.Instance.ConnGlobal);
                }

                DictionaryMgr.Instance.RefreshBusinessPartnersDictionary();

                MisCfgTypeList = BaseBindableDbEntity.GetList<MisCfgType>(Global.Instance.ConnGlobal);
            })
            .ContinueWith(antecendent =>
            {
                Dictionary_BusinessPartnersCodes = DictionaryMgr.Instance.Dictionary_BusinessPartners.ToDictionary(i => (object)i.BPA_Code, i => i.BPA_Code);
                Dictionary_BusinessPartnersDescriptions = DictionaryMgr.Instance.Dictionary_BusinessPartners.ToDictionary(i => (object)i.BPA_Code, i => i.BPA_Desc);

                if (!Order.LoadedFromDb)
                {
                    // imposto codice ordine di default
                    OrderCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                    DueDate = DateTime.Today;
                    Priority = PriorityList.FirstOrDefault();
                    MIT_Code = MisCfgTypeList.FirstOrDefault().MIT_Code;
                }
                else
                {
                    SetBusinessPartner(BPA_Code);
                }

                IsLoading = false;
            });

            await base.OnInitializeAsync(cancellationToken);
        }

        #endregion

        #region Public methods

        public bool CanSave
        {
            get { return !HasErrorsByGroup(); }
        }

        public async Task SaveAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to save current order?")))
                return;

            Order.ImportLines(OrdersDetailList.ToList());

            if (Order.Save() == false)
            {
                await Global.ErrorAsync(_windowManager, Order.LastError);

                return;
            }

            await TryCloseAsync(true);
        }

        public async Task CancelAsync()
        {
            await TryCloseAsync(false);
        }

        public bool CanNewRow
        {
            //get { return Order.LoadedFromDb; }
            get { return true; }
        }

        public async Task NewRowAsync()
        {
            var orderDetail = new MisOrdersDetail((SqlConnection)Global.Instance.ConnGlobal)
            {
                ODT_ORD_OrderCode = OrderCode,
                ODT_Line = _NextLineNum++
            };

            var vm = new InsertOrderDetailViewModel(_windowManager, _eventAggregator, orderDetail);

            if (await _windowManager.ShowDialogAsync(vm, settings: _windowSettings) == true)
            {
                OrdersDetailList.Add(vm.OrderDetail);
            }
        }

        public bool CanEditRow
        {
            get
            {
                //return Order.LoadedFromDb &&
                //    (Selected != null) &&
                //    !Selected.ODT_Closed;
                return (Selected != null) &&
                       !Selected.ODT_Closed;
            }
        }

        public async Task EditRowAsync()
        {
            var vm = new InsertOrderDetailViewModel(_windowManager, _eventAggregator, Selected.Clone() as MisOrdersDetail);

            if (await _windowManager.ShowDialogAsync(vm, settings: _windowSettings) == true)
            {
                Selected.Import(vm.OrderDetail);
            }
        }

        public bool CanDeleteRow
        {
            get
            {
                return (Selected != null) &&
                       !Selected.ODT_Closed;
            }
        }

        public void DeleteRow()
        {
            OrdersDetailList.Remove(Selected);
            Selected = null;
        }

        #endregion

        #region Private Methods

        private void SetBusinessPartner(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return;
            }

            var item = DictionaryMgr.Instance.Dictionary_BusinessPartners.FirstOrDefault(i => i.BPA_Code.TrimUI() == code.TrimUI());
            if (item == null)
            {
                return;
            }

            BPA_Code = item.BPA_Code;
            BPA_Desc = item.BPA_Desc;
        }

        #endregion

        #region Events

        public void CodeAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null)
            {
                return;
            }

            SetBusinessPartner(element.Value.ToString());
        }

        public void DescriptionAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null)
            {
                return;
            }

            SetBusinessPartner(element.Value.ToString());
        }

        #endregion
    }
}
