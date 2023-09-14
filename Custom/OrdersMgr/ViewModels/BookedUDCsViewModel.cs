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
    [Export(typeof(BookedUDCsViewModel))]
    class BookedUDCsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private DataTable _UDCs = null;
        private bool _IsLoading = false;

        #endregion

        #region Properties

        public AgilogDll.MisOrder Order { get; private set; }

        public DataTable UDCs
        {
            get { return _UDCs; }
            set
            {
                _UDCs = value;
                NotifyOfPropertyChange(() => UDCs);
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
        public BookedUDCsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgilogDll.MisOrder order)
        {
            DisplayName = Lang.Tl("Booked UDCs for") + " " + order.ORD_OrderCode;

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Order = order;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                string query = $@"SELECT EXE_ODT_Line,
	                                     UDC_Code, 
                                         UDC_UDT_Code, 
                                         UDT_Desc, 
                                         MIN(UCM_Index) 'UCM_Index', 
                                         UDC_Num_Ucm,
	                                     UDC_Wrapped,
                                         LOC_CEL_ID,
                                         CEL_X,
                                         CEL_Y,
                                         LOC_Z, 
                                         LOC_W,
	                                     ITM_Code,
	                                     ITM_Desc,
	                                     SUM(UCM_Stock) 'UCM_Stock'
                                    FROM mfn_tabBookedUDCs ({Order.ORD_OrderCode.SqlFormat()})
                                    --WHERE LOC_CEL_Id IS NOT NULL
                                    GROUP BY EXE_ODT_Line,
	                                         UDC_CODE, 
                                             UDC_UDT_CODE, 
                                             UDT_DESC, 
                                             UDC_Num_Ucm,
	                                         UDC_Wrapped,
                                             LOC_CEL_ID,
                                             CEL_X,
                                             CEL_Y,
                                             LOC_Z, 
                                             LOC_W,
	                                         ITM_Code,
	                                         ITM_Desc";

                UDCs = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);
            });

            IsLoading = false;

            await base.OnInitializeAsync(cancellationToken);
        }

        #endregion
    }
}
