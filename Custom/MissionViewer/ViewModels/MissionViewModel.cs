using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MissionViewer.ViewModels
{
    class MissionViewModel : Screen
    {
        #region Members
        private SqlConnection _conn;
        private object _lockObj = new object();

        private bool _IsLoading = false;
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

        public long MIS_Id { get; set; }

        public string MIS_UDC_Code { get; set; }

        public string MIS_MIT_Code { get; set; }

        public string MIS_ODT_ORD_OrderCode { get; set; }

        public int? MIS_ODT_Line { get; set; }

        public DateTime Start { get; set; }

        public DateTime LastChange { get; set; }

        public ObservableCollection<BckMission> BackupLines { get; } = new ObservableCollection<BckMission>(); 
        #endregion

        public MissionViewModel()
        {
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);

            BindingOperations.EnableCollectionSynchronization(BackupLines, _lockObj);
        }

        public async Task LoadBackupLinesAsync()
        {
            IsLoading = true;

            await Task.Factory.StartNew(() =>
            {
                OnUIThread(() => BackupLines.Clear());

                var records = BaseBindableDbEntity.GetList<BckMission>(_conn, $"WHERE MIS_Id = {MIS_Id.SqlFormat()}").OrderBy(x => x.BCK_Time).ToList();

                OnUIThread(() => records.ForEach(x => BackupLines.Add(x)));
            });

            IsLoading = false;
        }
    }
}
