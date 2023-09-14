using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhsViewer.ViewModels
{
    [Export(typeof(BayViewModel))]
    class BayViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private object _lockObj = new object();

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private string _Warehouse;
        private int _AisleNum;

        #endregion

        #region Properties

        public ObservableCollection<StcBay> BayList { get; protected set; }

        public int AisleNum
        {
            get { return _AisleNum; }
            set
            {
                if (_AisleNum != value)
                {
                    _AisleNum = value;
                    NotifyOfPropertyChange(() => AisleNum);

                    LoadDataAsync();
                }
            }
        }

        public bool HasPermission { get { return Global.Instance.CheckUserPerm(); } }

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
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

        #endregion

        #region Constructor

        [ImportingConstructor]
        public BayViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, string warehouse, int aisle)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _Warehouse = warehouse;
            _AisleNum = aisle;
        }

        #endregion

        #region ViewModel Override

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            BayList = new ObservableCollection<StcBay>();

            LoadDataAsync();
        }

        #endregion

        #region Public methods

        public async Task DriveAsync(StcBay bay)
        {
            if (! await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to run a DRIVE command?"))) return;

            IsLoading = true;

            string error = await bay.DriveAsync();

            IsLoading = false;

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
            }
            else
            {
                SnackBarMessage = Global.Instance.LangTl("Request sent successfully");
            }
        }

        #endregion

        #region Private methods

        private async void LoadDataAsync()
        {
            IsLoading = true;

            BayList.Clear();
            NotifyOfPropertyChange(() => BayList);

            await Task.Run(() =>
            {
                var condition = $"WHERE [STC_ASL_WHS_Code] = {_Warehouse.SqlFormat()} AND [STC_ASL_Num] = {_AisleNum.SqlFormat()}";
                var bays = StcBay.GetList((SqlConnection)Global.Instance.ConnGlobal, condition);

                OnUIThread(() => bays.ForEach(b => BayList.Add(b)));

                var machines = StcMachine.GetList<StcMachine>((SqlConnection)Global.Instance.ConnGlobal, condition);
                DisplayName = $"{Global.Instance.LangTl("Bays")} {string.Join(" - ", machines.Select(m => m.STC_PLN_Code).ToArray())}";
            });

            IsLoading = false;
        }

        #endregion
    }
}
