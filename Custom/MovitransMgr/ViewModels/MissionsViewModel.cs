using AgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace HandlingMgr.ViewModels
{
    [Export(typeof(MissionsViewModel))]
    class MissionsViewModel : Screen, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private List<MisMission> _Missions = null;
        private MisMission _SelectedMission = null;
        private List<LogEntry> _MissionLogs = null;
        private bool _IsLoading = false;
        private string _SnackBarMessage;

        #endregion

        #region Properties

        public List<MisMission> Missions
        {
            get { return _Missions; }
            set
            {
                _Missions = value;
                NotifyOfPropertyChange(() => Missions);
            }
        }

        public MisMission SelectedMission
        {
            get { return _SelectedMission; }
            set
            {
                _SelectedMission = value;
                RefreshLogs();

                NotifyOfPropertyChange(() => SelectedMission);
            }
        }

        public List<LogEntry> MissionLogs
        {
            get { return _MissionLogs; }
            set
            {
                _MissionLogs = value;
                NotifyOfPropertyChange(() => MissionLogs);
            }
        }

        public bool CanStart
        {
            get { return false; }
        }

        public bool CanStop
        {
            get { return false; }
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

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MissionsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Job list");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;
        }

        #endregion

        #region ViewModel Override

        protected override void OnInitialize()
        {
            RefreshAll();

            base.OnInitialize();
        }

        #endregion

        #region Public methods

        public void RefreshAll()
        {
            Task.Run(() =>
            {
                IsLoading = true;

                JobsList = EnginesMgr.Instance.Jobs;

                SelectedJob = null;
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });
        }

        public void Start()
        {
            if (SelectedJob.Start())
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
            }
        }

        public void Stop()
        {
            if (SelectedJob.Stop())
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
            }
        }

        public void Enable()
        {
            if (SelectedJob.SetEnabling(true))
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
            }
        }

        public void Disable()
        {
            if (SelectedJob.SetEnabling(false))
            {
                NotifyOfPropertyChange(() => CanStart);
                NotifyOfPropertyChange(() => CanStop);
                NotifyOfPropertyChange(() => CanEnable);
                NotifyOfPropertyChange(() => CanDisable);
            }
        }

        #endregion

        #region Private methods

        private void RefreshLogs()
        {
            Task.Run(() =>
            {
                if (_SelectedMission != null)
                {
                    IsLoading = true;

                    MissionLogs = Logger.GetLogs(SelectedJob.Code, 100);
                }
                else
                {
                    MissionLogs = Logger.GetLogs(Global.Instance.APP_Code, 100);
                }
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });
        }

        #endregion

        #region Global Events

        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Ogni 10 secondi rinfresco i log
            if ((Global.Instance.Repetitions1Sec % 10) != 0) return;

            RefreshLogs();
        }

        public void Handle(bool message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
