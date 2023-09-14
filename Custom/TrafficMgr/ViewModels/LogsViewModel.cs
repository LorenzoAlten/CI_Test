using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace TrafficMgr.ViewModels
{
    [Export(typeof(TelegramsViewModel))]
    public class LogsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private List<LogEntry> _logs = null;
        private bool _IsLoading = false;

        #endregion

        #region Properties

        public string ItemCode { get; private set; }
        public string UdcCode { get; private set; }

        public List<LogEntry> Logs
        {
            get { return _logs; }
            set
            {
                _logs = value;
                NotifyOfPropertyChange(() => Logs);
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
        public LogsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Lang.Tl("Logs history");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            _logs = new List<LogEntry>();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            IsLoading = true;

            Task.Run(() =>
            {
                _logs.Clear();

                var query = $@"SELECT TOP 1000 * FROM LOG_WORK
                               WHERE LOG_App = 'TrafficMgr'
                               ORDER BY LOG_Id DESC";
                var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);
                if (dt == null || dt.Rows.Count <= 0) return;

                foreach (DataRow row in dt.Rows)
                {
                    _logs.Add(new LogEntry
                    {
                        DateTime = row.GetValueDT("LOG_DateTime"),
                        Message = row.GetValue("LOG_Message"),
                        User = row.GetValue("LOG_User"),
                        Application = row.GetValue("LOG_App"),
                        DVC_Code = row.GetValue("LOG_DVC_Code"),
                        Level = (LogLevels)row.GetValueI("LOG_Level")
                    });
                }
            }).ContinueWith(antecedent =>
            {
                Application.Current.Dispatcher.Invoke(() => NotifyOfPropertyChange(() => Logs));
                IsLoading = false;
            });
        }

        #endregion
    }
}
