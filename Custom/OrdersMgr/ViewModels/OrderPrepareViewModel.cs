using mSwAgilogDll;
using mSwAgilogDll.ViewModels;
using Caliburn.Micro;
using mSwDllWPFUtils;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.SqlClient;
using mSwDllUtils;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(OrderPrepareViewModel))]
    public class OrderPrepareViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private RunUtils _utils;

        private string _CustomerCode;
        private string _CustomerName;
        private bool _IsLoading = false;
        private bool _canLaunch = false;
        private bool _operationDone = false;
        private string _retVal;
        private object _lockObj = new object();

        private MisOrder _order;
        private int? _bayNum;

        #endregion

        public string CustomerCode
        {
            get { return _CustomerCode; }
            set
            {
                _CustomerCode = value;
                NotifyOfPropertyChange(() => CustomerCode);
            }
        }
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                _CustomerName = value;
                NotifyOfPropertyChange(() => CustomerName);
            }
        }
        /// <summary>
        /// Lista di clienti
        /// </summary>
        public ObservableCollection<BusinessPartner> CustomersList { get; set; } = new ObservableCollection<BusinessPartner>();

        public bool OperationDone
        {
            get { return _operationDone; }
            set
            {
                _operationDone = value;
                NotifyOfPropertyChange(() => OperationDone);
            }
        }

        public bool CanLaunch
        {
            get { return _canLaunch; }
            set
            {
                _canLaunch = value;
                NotifyOfPropertyChange(() => CanLaunch);
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

        public string RetVal
        {
            get { return _retVal; }
            set
            {
                _retVal = value;
                NotifyOfPropertyChange(() => RetVal);
            }
        }

        #region Constructor

        [ImportingConstructor]
        public OrderPrepareViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, MisOrder order, int? bayNum = null)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            _order = order;
            _bayNum = bayNum;
            CanLaunch = false;
            OperationDone = false;
            RetVal = null;

            BindingOperations.EnableCollectionSynchronization(CustomersList, _lockObj);
            LoadCustomersList();
        }

        #endregion

        #region Override

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            IsLoading = true;
            int locations = await Task.Run(() => { return _utils.RunOrder_CheckSimulaLoc(); });
            IsLoading = false;

            OperationDone = true;

            RetVal = string.Format(Global.Instance.LangTl("{0} preparation channels available"), locations);

            CanLaunch = locations > 0;
        }

        #endregion

        #region Private Methods

        public void LoadCustomersList()
        {
            CustomersList.Clear();
            MasterDataManager.Instance.CustomersList.ForEach(x => CustomersList.Add(x));
        }

        #endregion

        #region Public Methods

        public async Task SelectCustomerAsync()
        {
            IsLoading = true;

            var vm = new SelectCustomerViewModel(_windowManager, _eventAggregator, MasterDataManager.Instance.CustomersList);

            IsLoading = false;

            var retVal = await _windowManager.ShowDialogAsync(vm);

            if (retVal == null || !retVal.Value) return;

            CustomerCode = vm.SelectedCustomer.BPA_Code.ToString();
        }

        public async Task LaunchAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerCode))
            {
                await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("You must select a customer to link your preparation"));
                return;
            }
            
            await TryCloseAsync(true);
        }

        #endregion
    }
}
