using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace TrafficMgr.ViewModels
{
    [Export(typeof(TelegramsViewModel))]
    public class TelegramsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private object _lockObj = new object();

        private bool _IsLoading = false;

        #endregion

        #region Properties

        public string Filter { get; set; }

        public string ItemCode { get; private set; }
        public string UdcCode { get; private set; }

        public ObservableCollection<MsgEntry> Entries { get; } = new ObservableCollection<MsgEntry>();

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
        public TelegramsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Lang.Tl("Telegrams history");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            BindingOperations.EnableCollectionSynchronization(Entries, _lockObj);
        }

        #endregion

        #region ViewModel Override

        protected override async void OnViewLoaded(object view)
        {
            await RefreshAsync();
        }

        #endregion

        #region Public Methods

        public async Task RefreshAsync()
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                try
                {
                    Entries.Clear();

                    var results = Manager.Instance.MsgEntries;

                    if (!string.IsNullOrEmpty(Filter))
                    {
                        results = results.Where(x => x.Message.ToUpper().Contains(Filter.ToUpper())).ToList();
                    }

                    results.OrderByDescending(e => e.Timestamp).ToList().ForEach(x => Entries.Add(x));
                }
                catch (Exception ex)
                {

                }
            });

            IsLoading = true;
        }

        public async Task ExportAsync()
        {
            if (Entries.Count <= 0)
            {
                await Global.AlertAsync(_windowManager, Global.Instance.LangTl("No data to export. Try changing some filters"));
                return;
            }

            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.Filter = "CSV file(*.csv)| *.csv | All Files(*.*) | *.* ";
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".csv";
            fileDialog.FileName = $"Telegrams_{DateTime.Now:yyyyMMdd-HHmm}";

            var retVal = fileDialog.ShowDialog();
            if (!retVal.HasValue || !retVal.Value) return;

            try
            {
                IsLoading = true;

                string fileName = fileDialog.FileName;
                List<string> fileContent = new List<string>();

                await Task.Run(() =>
                {
                    // Header
                    string header = $"DATETIME,SENDER,DESTINATION,MESSAGE";

                    fileContent.Add(header);

                    foreach (MsgEntry entry in Entries)
                    {
                        string csv = "";
                        csv += $"{entry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss:fff")},";
                        csv += $"{entry.Sender},";
                        csv += $"{entry.Dest},";
                        csv += $"{entry.Message}";

                        fileContent.Add(csv);
                    }
                })
                .ContinueWith(antecendent =>
                {
                    IsLoading = false;
                });

                System.IO.File.WriteAllLines(fileName, fileContent.ToArray());
                await Global.AlertAsync(_windowManager, Global.Instance.LangTl("Data exported successfully"));
            }
            catch (Exception ex)
            {
                await Global.ErrorAsync(_windowManager, ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void FilterTelegrams(KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                RefreshAsync();
            }
        }

        #endregion
    }
}
