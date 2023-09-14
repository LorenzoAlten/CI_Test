using mSwAgilogDll;
using mSwAgilogDll.ViewModels;
using Caliburn.Micro;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using mSwAgilogDll.SEW;
using AgvMgr.Entites;
using AgvMgr.AppData;
using System.ComponentModel;
using System.Threading;
using System;
using System.Windows.Data;

namespace AgvMgr.ViewModels
{
    [Export(typeof(MissionsViewModel))]
    public class MissionsViewModel : Screen, IHandle<string>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private MisMissionAgv _selectedMission = null;
        private bool _IsLoading = false;
        private string _SnackBarMessage;
        private object _lockObj = new object();

        private BackgroundWorker _MissionsWorker;

        #endregion

        #region Properties

        public MisMissionAgv SelectedMission
        {
            get { return _selectedMission; }
            set
            {
                _selectedMission = value;
                NotifyOfPropertyChange(() => SelectedMission);
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

        public ICommand ErrorCommand { get; }

        public ObservableCollection<MisMissionAgv> MissionsList { get; } = new ObservableCollection<MisMissionAgv>();

        //public ICommand RowAcceptCommand { get; }

        public ICommand RowAbortCommand { get; }

        public ICommand RowCompleteCommand { get; }

        public ICommand RowDetailsCommand { get; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MissionsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _MissionsWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            _MissionsWorker.DoWork += MissionsWorker_DoWork;

            //RowAcceptCommand = new CustomAsyncCommandImplementation(AcceptAsync);
            RowAbortCommand = new CustomAsyncCommandImplementation(AbortAsync);
            RowCompleteCommand = new CustomAsyncCommandImplementation(CompleteAsync);
            RowDetailsCommand = new CustomAsyncCommandImplementation(UdcDetails);

            BindingOperations.EnableCollectionSynchronization(MissionsList, _lockObj);
        }

        #endregion

        #region ViewModel Override

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_MissionsWorker.IsBusy)
            {
                _MissionsWorker.RunWorkerAsync();
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _MissionsWorker.CancelAsync();

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Public methods

        public void RefreshAll()
        {
            LoadMissions();
        }

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            if (message == "REFRESH_MISSION")
            {
                LoadMissions();
            }

            await Task.CompletedTask;
        }

        //public async Task AcceptAsync()
        //{
        //    if (!Global.Confirm(_windowManager, Global.Instance.LangTl("Do you really want to handle the error automatically?")))
        //        return;

        //    IsLoading = true;

        //    bool retVal = await Task.Run(() =>
        //    {
        //        SelectedMission.MIS_ERR_UserAction = EMisErrorUserActions.ACCEPT.ToString();
        //        if (!SelectedMission.Update())
        //        {
        //            Global.Error(_windowManager, SelectedMission.LastError);
        //            return false;
        //        }
        //        return true;
        //    });

        //    IsLoading = false;

        //    if (retVal)
        //    {
        //        SnackBarMessage = Global.Instance.LangTl("Operation completed successfully");
        //        RefreshAll();
        //    }
        //}

        public async Task AbortAsync()
        {
            if (SelectedMission == null) return;

            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to Abort the current mission?")))
                return;

            IsLoading = true;

            bool retVal = await Task.Run(() =>
            {
                //if (!string.IsNullOrWhiteSpace(SelectedMission.MIS_ERR_Current_Location))
                //    SelectedMission.MIS_ERR_UserAction = EMisErrorUserActions.ABORT.ToString();
                //else
                SelectedMission.AbortRequest = EMisActionRequests.W;
                if (!SelectedMission.Update())
                {
                    Global.ErrorAsync(_windowManager, SelectedMission.LastError);
                    return false;
                }
                return true;
            });

            IsLoading = false;

            if (retVal)
            {
                SnackBarMessage = Global.Instance.LangTl("Operation completed successfully");
                RefreshAll();
            }
        }

        public async Task CompleteAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to Complete the mission? The mission will be considered as if it had ended successfully")))
                return;

            IsLoading = true;

            bool retVal = await Task.Run(() =>
             {
                 SelectedMission.MIS_ERR_UserAction = EMisErrorUserActions.COMPLETE.ToString();
                 if (!SelectedMission.Update())
                 {
                     Global.ErrorAsync(_windowManager, SelectedMission.LastError);
                     return false;
                 }
                 return true;
             });

            IsLoading = false;

            if (retVal)
            {
                SnackBarMessage = Global.Instance.LangTl("Operation completed successfully");
                RefreshAll();
            }
        }

        public async Task UdcDetails()
        {
            if (SelectedMission == null) return;

            UdcsViewModel vm = new UdcsViewModel(_windowManager, _eventAggregator, SelectedMission.MIS_UDC_Code, EUdcPositions.Both);
            await _windowManager.ShowWindowAsync(vm);
        }

        public void MissionMouseDoubleClick()
        {
            if (SelectedMission == null) return;

            Clipboard.SetText(SelectedMission.MIS_UDC_Code);
        }

        #endregion

        #region Private methods

        private void MissionsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_MissionsWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                LoadMissions();

                Thread.Sleep(500);
            }
        }

        private void LoadMissions()
        {
            lock (MissionsList)
            {
                MissionsList.Clear();

                var missionsList = Common.Instance.MissionsList;

                missionsList.ForEach(m =>
                {
                    m.Agv = Common.Instance.Agvs.FirstOrDefault(a => a.AgvRequest.AGV_Mission == m.MIS_Id)?.AGV_Code;
                    MissionsList.Add(m);
                });
            }

            if (SelectedMission != null && !MissionsList.Contains(SelectedMission))
                SelectedMission = null;

            IsLoading = false;
        }

        #endregion
    }
}
