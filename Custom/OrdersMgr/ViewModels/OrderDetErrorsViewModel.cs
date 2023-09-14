using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(OrderDetErrorsViewModel))]
    class OrderDetErrorsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private DataTable _errors = null;
        private bool _IsLoading = false;

        #endregion

        #region Properties

        public AgilogDll.MisOrdersDetail Detail { get; private set; }

        public DataTable Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                NotifyOfPropertyChange(() => Errors);
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
        public OrderDetErrorsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgilogDll.MisOrdersDetail detail)
        {
            DisplayName = Lang.Tl("Line errors");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Detail = detail;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                IsLoading = true;

                string query = $@"SELECT [ERR_Time], [ERR_Error] FROM [MIS_ORDERS_DETAILS_ERR]
                                  WHERE [ERR_ODT_ORD_OrderCode] = {Detail.ODT_ORD_OrderCode.SqlFormat()}
                                  AND [ERR_ODT_Line] = {Detail.ODT_Line.SqlFormat()}
                                  ORDER BY [ERR_Time] DESC";

                Errors = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });

            await base.OnInitializeAsync(cancellationToken);
        }

        #endregion
    }
}
