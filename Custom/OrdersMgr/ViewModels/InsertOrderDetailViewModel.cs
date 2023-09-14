using mSwAgilogDll;
using Caliburn.Micro;
using Caliburn.Micro.Validation;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(InsertOrderDetailViewModel))]
    class InsertOrderDetailViewModel : ValidatingScreen<InsertOrderDetailViewModel>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private MisOrdersDetail _OrderDetail = null;
        private string _ITM_Desc;
        private List<HndBayBay> _BaysList;
        private List<UdcUdc> _UdcList;
        private Dictionary<object, string> _Dictionary_ItemCodes;
        private Dictionary<object, string> _Dictionary_ItemDescriptions;
        private bool _IsLoading = false;

        #endregion

        #region Properties

        public MisOrdersDetail OrderDetail
        {
            get { return _OrderDetail; }
            set
            {
                _OrderDetail = value;
                NotifyOfPropertyChange(() => OrderDetail);
            }
        }

        /// <summary>
        /// Codice articolo
        /// </summary>
        [Required(ErrorMessage = "Field is required")]
        public string ITM_Code
        {
            get { return _OrderDetail.ODT_ITM_Code; }
            set
            {
                _OrderDetail.ODT_ITM_Code = value;
                NotifyOfPropertyChange(() => ITM_Code);
                NotifyOfPropertyChange(() => CanSave);

                UdcList = null;
            }
        }
        /// <summary>
        /// Descrizione articolo
        /// </summary>
        public string ITM_Desc
        {
            get { return _ITM_Desc; }
            set
            {
                _ITM_Desc = value;
                NotifyOfPropertyChange(() => ITM_Desc);

                UdcList = null;
            }
        }

        /// <summary>
        /// Qty requested
        /// </summary>
        [Required(ErrorMessage = "Field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a value bigger than {1}")]
        public decimal QtyR
        {
            get { return _OrderDetail.ODT_QtyR; }
            set
            {
                _OrderDetail.ODT_QtyR = value;
                NotifyOfPropertyChange(() => QtyR);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        /// <summary>
        /// Priorità
        /// </summary>
        public int? Priority
        {
            get { return _OrderDetail.ODT_Priority; }
            set
            {
                _OrderDetail.ODT_Priority = value;
                NotifyOfPropertyChange(() => Priority);
            }
        }

        /// <summary>
        /// Codice UDC
        /// </summary>
        public string UDC_Code
        {
            get { return _OrderDetail.ODT_UDC_Code; }
            set
            {
                _OrderDetail.ODT_UDC_Code = value;
                NotifyOfPropertyChange(() => UDC_Code);
            }
        }

        /// <summary>
        /// Scomparto
        /// </summary>
        public int? UCM_Index
        {
            get { return _OrderDetail.ODT_UCM_Index; }
            set
            {
                _OrderDetail.ODT_UCM_Index = value;
                NotifyOfPropertyChange(() => UCM_Index);
            }
        }

        /// <summary>
        /// Numero baia
        /// </summary>
        [Required(ErrorMessage = "Field is required")]
        public int? BAY_Num
        {
            get { return _OrderDetail.ODT_BAY_Num; }
            set
            {
                _OrderDetail.ODT_BAY_Num = value;
                NotifyOfPropertyChange(() => BAY_Num);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        /// <summary>
        /// Notes of the supervisor for the operator in bay or of the management system to the supervisor or the operator in bay
        /// </summary>
        public string Notes
        {
            get { return _OrderDetail.ODT_Notes; }
            set
            {
                _OrderDetail.ODT_Notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        /// <summary>
        /// Lista UDC
        /// </summary>
        public List<UdcUdc> UdcList
        {
            get { return _UdcList; }
            set
            {
                _UdcList = value;
                NotifyOfPropertyChange(() => UdcList);
            }
        }

        /// <summary>
        /// Lista baie
        /// </summary>
        public List<HndBayBay> BaysList
        {
            get { return _BaysList; }
            set
            {
                _BaysList = value;
                NotifyOfPropertyChange(() => BaysList);
            }
        }

        /// <summary>
        /// Lista delle priorità accettate
        /// </summary>
        public IEnumerable<int> PriorityList
        {
            get { return Enumerable.Range(0, 11); }
        }

        /// <summary>
        /// Lista dei codici degli articoli
        /// </summary>
        public Dictionary<object, string> Dictionary_ItemCodes
        {
            get { return _Dictionary_ItemCodes; }
            set
            {
                _Dictionary_ItemCodes = value;
                NotifyOfPropertyChange(() => Dictionary_ItemCodes);
            }
        }

        /// <summary>
        /// Lista delle descrizioni degli articoli
        /// </summary>
        public Dictionary<object, string> Dictionary_ItemDescriptions
        {
            get { return _Dictionary_ItemDescriptions; }
            set
            {
                _Dictionary_ItemDescriptions = value;
                NotifyOfPropertyChange(() => Dictionary_ItemDescriptions);
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
        public InsertOrderDetailViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, MisOrdersDetail orderDetail = null)
        {
            DisplayName = !orderDetail.LoadedFromDb ? Global.Instance.LangTl("New order row") : Global.Instance.LangTl("Edit order row");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            OrderDetail = orderDetail;
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

                DictionaryMgr.Instance.RefreshItemDictionaries();

                BaysList = BaseBindableDbEntity.GetList<HndBayBay>(Global.Instance.ConnGlobal).Where(b => b.BAY_Enabled).ToList();
            })
            .ContinueWith(antecendent =>
            {
                Dictionary_ItemCodes = DictionaryMgr.Instance.Dictionary_Items.ToDictionary(i => (object)i.ITM_Code, i => i.ITM_Code);
                Dictionary_ItemDescriptions = DictionaryMgr.Instance.Dictionary_Items.ToDictionary(i => (object)i.ITM_Code, i => i.ITM_Desc);


                if (!BAY_Num.HasValue) BAY_Num = BaysList.FirstOrDefault().BAY_Num;
                if (!Priority.HasValue) Priority = PriorityList.FirstOrDefault();
                SetItem(ITM_Code);

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
            await TryCloseAsync(true);
        }

        public async Task CancelAsync()
        {
            OrderDetail = null;

            await TryCloseAsync(false);
        }

        #endregion

        #region Private Methods

        private void SetItem(string itemCode)
        {
            if (string.IsNullOrWhiteSpace(itemCode))
            {
                return;
            }

            var item = DictionaryMgr.Instance.Dictionary_Items.FirstOrDefault(i => i.ITM_Code.TrimUI() == itemCode.TrimUI());
            if (item == null)
            {
                return;
            }

            ITM_Code = item.ITM_Code;
            ITM_Desc = item.ITM_Desc;

            // Carico le UDC che contengono l'articolo
            Task.Run(() =>
            {
                IsLoading = true;
                UdcList = UdcUdc.GetList(
                    Global.Instance.ConnGlobal,
                    $@"WHERE EXISTS (SELECT [UCM_Index] FROM [UDC_COMPARTMENTS]
                                     WHERE [UCM_UDC_Code] = [UDC_Code]
                                     AND [UCM_ITM_Code] = {ITM_Code.SqlFormat()})");
                IsLoading = false;
            });
        }

        #endregion

        #region Events

        public void ItemCodeAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null)
            {
                return;
            }

            SetItem(element.Value.ToString());
        }

        public void ItemDescriptionAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null)
            {
                return;
            }

            SetItem(element.Value.ToString());
        }

        #endregion
    }
}
