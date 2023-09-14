using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using mSwDllMFC;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using mSwDllGrpc;

namespace TrafficMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    class AppViewModel : Conductor<Screen>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _LoadingIsVisible;
        private DateTime _Now = DateTime.Now;

        private int _telegramSaveInterval = 30;
        private bool _saving;
        private DateTime _lastSave;

        #endregion

        #region Properties

        public Screen Missions { get; private set; }
        public bool IsLoading
        {
            get { return _LoadingIsVisible; }
            private set
            {
                _LoadingIsVisible = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }
        public DateTime Now
        {
            get { return _Now; }
            private set
            {
                _Now = value;
                NotifyOfPropertyChange(() => Now);
            }
        }
        public List<BaseRuotineComponent> Managers { get; set; }
        public ObservableCollection<TrafficManager> ManagerCollection { get; set; }
        public ObservableCollection<BaseRuotineComponent> GenericManagerCollection { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = "Agilog Traffic Manager";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Missions = new MainPageViewModel(_windowManager, _eventAggregator);
            Missions.ConductWith(this);

            LoadManagers();

            _lastSave = DateTime.Now;
        }

        #endregion

        #region ViewModel Override

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;

                // Chiudo il servizio WCF
                if (TelegramServiceMgr.Instance != null)
                    TelegramServiceMgr.Instance.Dispose();

                Managers.ForEach(m => m.Dispose());
            }

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Global Events

        private async void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Aggiorno la data/ora
            Now = DateTime.Now;

            if (_telegramSaveInterval <= 0 ||
                Global.Instance.Repetitions1Sec % _telegramSaveInterval != 0 ||
                _saving)
                return;

            _saving = true;

            await Task.Run(() =>
            {
                try
                {
                    if (Manager.Instance.MsgEntries == null) return;

                    var results = new List<MsgEntry>(Manager.Instance.MsgEntries);
                    DateTime now = DateTime.Now;
                    results.RemoveAll(m => m.Timestamp <= _lastSave);
                    if (results.Count <= 0) return;

                    string query = @"
                        INSERT INTO [dbo].[HIS_TELEGRAMS]
                           ([TEL_Timestamp]
                           ,[TEL_Sender]
                           ,[TEL_Receiver]
                           ,[TEL_Message])";

                    for (int i = 0; i < results.Count; i++)
                    {
                        var entry = results[i];

                        if (i > 0)
                        {
                            query += @"
                        UNION ALL";
                        }

                        query += $@"
                        SELECT {DbUtils.SqlFormat(entry.Timestamp)}, {DbUtils.SqlFormat(entry.Sender)}, {DbUtils.SqlFormat(entry.Dest)}, {DbUtils.SqlFormat(entry.Message)}";
                    }

                    var conn = DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                    DbUtils.ExecuteNonQuery(query, conn);

                    _lastSave = now;
                }
                catch { }
            });

            _saving = false;
        }

        #endregion

        #region Private Methods

        private async Task LoadManagers()
        {
            IsLoading = true;
            Managers = null;

            await Task.Run(() =>
            {
                DateTime now = DateTime.Now;

                Managers = TrafficManager.GetList((SqlConnection)Global.Instance.ConnGlobal, Global.Instance.DVC_Id, AppDomain.CurrentDomain.FriendlyName);
                while (Managers.Any(m => !m.InitComplete || (m is TrafficManager && (m as TrafficManager).ControllerCollection.Any(c => !c.InitComplete))))
                {
                    if (DateTime.Now.Subtract(now).TotalSeconds > 20)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Cannot initialize traffic managers. Check application Logs"));
                            Environment.Exit(0);
                        });
                    }
                    Task.Delay(10);
                }
            })
            .ContinueWith(antecendent =>
            {
                IsLoading = false;
            });

            if (Managers == null)
            {
                // Notifico ai ViewModel l'inizializzazione fallita
                await _eventAggregator.PublishOnUIThreadAsync(false);

                await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Cannot initialize traffic managers. Check application Logs"));
                Environment.Exit(0);
            }

            // Valorizzo la collection interna dei Manager per il Binding
            var trafficManagers = Managers.Where(m => m is TrafficManager).Select(m => m as TrafficManager).ToList();

            ManagerCollection = new ObservableCollection<TrafficManager>();
            NotifyOfPropertyChange(() => ManagerCollection);
            trafficManagers.ForEach(m => ManagerCollection.Add(m));

            GenericManagerCollection = new ObservableCollection<BaseRuotineComponent>();
            NotifyOfPropertyChange(() => GenericManagerCollection);
            Managers.Except(trafficManagers).ToList().ForEach(m => GenericManagerCollection.Add(m));

            // Copio la lista dei Managers nelle utility condivise
            TrafficUtils.Managers = new List<TrafficManager>(trafficManagers);

            // Istanzio le comunicazioni standard se necessario
            if (trafficManagers.SelectMany(m => m.ControllerCollection).Distinct().Any(c => c.NeedStandardCommunications))
            {
                //Global.Instance.OnCommsInitialized += Global_OnCommsInitialized;
                Global.Instance.OnFirstTimeDone += Global_OnFirstTimeDone;
                Global.Instance.StartUpSoloComms(Global.Instance.MenuOrder, false, true);
            }

            // Notifico ai ViewModel l'inizializzazione avvenuta
            await _eventAggregator.PublishOnUIThreadAsync(true);
        }

        private void Global_OnFirstTimeDone(object sender, GenericEventArgs e)
        {
            TrafficUtils.Managers.SelectMany(m => m.ControllerCollection).Distinct().ToList().ForEach(c => c.InitStandardReadings());
        }

        //private void Global_OnCommsInitialized(object sender, GenericEventArgs e)
        //{
        //    TrafficUtils.Managers.SelectMany(m => m.ControllerCollection).Distinct().ToList().ForEach(c => c.InitStandardReadings());
        //}

        #endregion
    }
}
