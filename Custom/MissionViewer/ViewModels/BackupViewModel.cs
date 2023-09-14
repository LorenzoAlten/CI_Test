using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Threading;

namespace MissionViewer.ViewModels
{
    [Export(typeof(BackupViewModel))]
    class BackupViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private object _lockObj = new object();
        private SqlConnection _conn;

        private bool _IsLoading = false;
        private bool _CanLoadData;
        private DateTime _FromDate = DateTime.Today;
        private DateTime _ToDate = DateTime.Today;
        private DateTime _FromTime = DateTime.Today;
        private DateTime _ToTime = DateTime.Now;
        private int? _MisId;
        private string _UdcCode;
        private string _OrderCode;

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

        public bool CanLoadData
        {
            get { return _CanLoadData; }
            set
            {
                _CanLoadData = value;
                NotifyOfPropertyChange(() => CanLoadData);
            }
        }

        public int? MisId
        {
            get { return _MisId; }
            set
            {
                _MisId = value;
                NotifyOfPropertyChange(() => MisId);
            }
        }

        public string UdcCode
        {
            get { return _UdcCode; }
            set
            {
                _UdcCode = value;
                NotifyOfPropertyChange(() => UdcCode);
            }
        }        

        public string OrderCode
        {
            get { return _OrderCode; }
            set
            {
                _OrderCode = value;
                NotifyOfPropertyChange(() => OrderCode);
            }
        }

        public DateTime FromDate
        {
            get { return _FromDate; }
            set
            {
                _FromDate = value;
                NotifyOfPropertyChange(() => FromDate);
            }
        }

        public DateTime FromTime
        {
            get { return _FromTime; }
            set
            {
                _FromTime = value;
                NotifyOfPropertyChange(() => FromTime);
            }
        }

        public DateTime ToDate
        {
            get { return _ToDate; }
            set
            {
                _ToDate = value;
                NotifyOfPropertyChange(() => ToDate);
            }
        }

        public DateTime ToTime
        {
            get { return _ToTime; }
            set
            {
                _ToTime = value;
                NotifyOfPropertyChange(() => ToTime);
            }
        }

        public ObservableCollection<MissionViewModel> MissionsList { get; } = new ObservableCollection<MissionViewModel>();

        #endregion

        #region Constructor

        [ImportingConstructor]
        public BackupViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Backup Missions");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);

            BindingOperations.EnableCollectionSynchronization(MissionsList, _lockObj);
        }

        #endregion

        #region ViewModel Override

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadDataAsync();
        }

        #endregion

        private string GetWhereClause()
        {
            string where = "";

            if (MisId.HasValue)
            {
                where = $" AND MIS_Id = {MisId.SqlFormat()}";
            }

            if (!string.IsNullOrEmpty(UdcCode))
            {
                where = $" AND MIS_UDC_Code = {UdcCode.SqlFormat()}";
            }

            if (!string.IsNullOrEmpty(OrderCode))
            {
                where = $" AND MIS_ODT_ORD_OrderCode = {OrderCode.SqlFormat()}";
            }

            where = $"WHERE 0 = 0 {where}";

            return where;
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            CanLoadData = false;

            await Task.Factory.StartNew(() =>
            {
               MissionsList.Clear();

                var from = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, FromTime.Hour, FromTime.Minute, FromTime.Second);
                var to = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, ToTime.Hour, ToTime.Minute, ToTime.Second);

                var query = $"SELECT MIS_Id, MIS_UDC_Code, MIS_ODT_ORD_OrderCode, MIS_ODT_Line, MIS_MIT_Code, MIN(BCK_Time) AS 'Start', MAX(BCK_Time) AS 'LastChange'" + Environment.NewLine +
                            $"FROM BCK_MISSIONS" + Environment.NewLine +
                            $"{GetWhereClause()}" + Environment.NewLine +
                            $"GROUP BY MIS_Id, MIS_UDC_Code, MIS_ODT_ORD_OrderCode, MIS_ODT_Line, MIS_MIT_Code" + Environment.NewLine +
                            $"HAVING (MIN(BCK_Time) >= {from.SqlFormat()} AND MAX(BCK_Time) <= {to.SqlFormat()})" + Environment.NewLine +
                            $"ORDER BY MIS_Id";

                var dt = DbUtils.ExecuteDataTable(query, _conn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MissionsList.Add(new MissionViewModel
                        {
                            MIS_Id = row.GetValueI("MIS_Id"),
                            MIS_UDC_Code = row.GetValue("MIS_UDC_Code"),
                            MIS_ODT_ORD_OrderCode = row.GetValue("MIS_ODT_ORD_OrderCode"),
                            MIS_ODT_Line = row.GetValueNullI("MIS_ODT_Line"),
                            MIS_MIT_Code = row.GetValue("MIS_MIT_Code"),
                            Start = row.GetValueDT("Start"),
                            LastChange = row.GetValueDT("LastChange")
                        });
                    }
                }                    
            });

            CanLoadData = true;
            IsLoading = false;
        }
    }
}
