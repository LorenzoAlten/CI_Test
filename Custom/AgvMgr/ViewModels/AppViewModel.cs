using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using mSwAgilogDll;
using System.Data.SqlClient;
using System.Data;
using mSwDllMFC;
using System.Windows.Controls;
using System.Windows;
using mSwAgilogDll.SEW;
using mSwDllGraphics;
using System.Runtime.Serialization;
using System.IO;
using AgvMgr.AppData;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AgvMgr.ViewModels
{
    [Export(typeof(AppViewModel))]
    public class AppViewModel : Conductor<Screen>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private SqlConnection _conn;
        private AgilogDll.RunUtils _utils;

        private AgvLayoutViewModel _agvLayoutViewModel;

        private bool _menuOpened = false;

        private DateTime _Now = DateTime.Now;
        private bool _LoadingIsVisible;

        private BackgroundWorker _RefreshWorker;

        #endregion

        #region Properties
        public bool MenuOpened
        {
            get { return _menuOpened; }
            set
            {
                _menuOpened = value;
                NotifyOfPropertyChange(() => MenuOpened);
            }
        }

        public Screen AgvManager { get; private set; }

        public DateTime Now
        {
            get { return _Now; }
            private set
            {
                _Now = value;
                NotifyOfPropertyChange(() => Now);
            }
        }

        public bool LoadingIsVisible
        {
            get { return _LoadingIsVisible; }
            set
            {
                if (_LoadingIsVisible == value) return;

                _LoadingIsVisible = value;
                NotifyOfPropertyChange(() => LoadingIsVisible);
            }
        }

        protected List<AgvStation> AgvStationList { get; set; }

        public CfgManagerViewModel CfgManagerViewModel { get; private set; }
        public AgvManagerViewModel AgvManagerViewModel { get; private set; }
        
        #endregion

        #region Constructor

        [ImportingConstructor]
        public AppViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(_conn));

            _RefreshWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            _RefreshWorker.DoWork += RefreshWorker_DoWork;

            DisplayName = Global.Instance.LangTl("AGVs").TrimUI();

            Global.Instance.OnEvery100mSec += Global_OnEvery100mSec;
            Global.Instance.OnEvery1Minute += Global_OnEvery1Minute;

            CfgManagerViewModel = new CfgManagerViewModel(_windowManager, _eventAggregator);
            CfgManagerViewModel.ConductWith(this);
            AgvManagerViewModel = new AgvManagerViewModel(_windowManager, _eventAggregator);
            AgvManagerViewModel.ConductWith(this);
        }

        #endregion

        #region ViewModel Override

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (!InitApplicationParameters())
                Environment.Exit(0);

            Global.Instance.App_Activated();

            //base.OnInitialize();

            Common.Instance.LoadMissions();

            ActivateItemAsync(AgvManagerViewModel);

            string multimedia = Path.Combine(AppConfig.XmlRelativePath, AppConfig.GetXmlKeyValue("Graphics", "MultimediaPath"));

            Graphic3DTools.Init(multimedia);
            Graphic2DTools.Init(multimedia, false);

            _agvLayoutViewModel = new AgvLayoutViewModel(_windowManager, _eventAggregator, AgvStationList);

            return base.OnInitializeAsync(cancellationToken);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_RefreshWorker.IsBusy)
            {
                _RefreshWorker.RunWorkerAsync();
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _RefreshWorker.CancelAsync();

            if (close)
            {
                Global.Instance.OnEvery100mSec -= Global_OnEvery100mSec;
                Global.Instance.OnEvery1Minute -= Global_OnEvery1Minute;
            }

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Initialize

        private bool InitApplicationParameters()
        {
            try
            {
                Common.Instance.LoadAgvs();

                AgvStationList = BaseBindableDbEntity.GetList<AgvStation>(Global.Instance.ConnGlobal, "WHERE [MOD_HMT_Code] = 'AGVSTATION'");

                LoadingIsVisible = false;
            }
            catch (Exception ex)
            {
                Global.ErrorAsync(_windowManager, ex.Message);
                return false;
            }

            return true;
        }

        private void RefreshAgv()
        {
            try
            {
                List<AgvRequest> agvs = BaseBindableDbEntity.GetList<AgvRequest>(_conn, KeepConnectionOpen: true);

                lock (agvs)
                {
                    foreach (SEW_AGV agv in Common.Instance.Agvs)
                    {
                        var newAgv = agvs.FirstOrDefault(a => a.AGV_Code == agv.AGV_Code);

                        if (newAgv != null)
                        {
                            agv.AgvRequest.State = newAgv.State;
                            agv.AgvRequest.AGV_Mission = newAgv.AGV_Mission;

                            if (newAgv.AGV_Mission_State != agv.AgvRequest.AGV_Mission_State)
                                agv.AgvRequest.MissionState = newAgv.MissionState;

                            agv.AgvRequest.AGV_Loaded = newAgv.AGV_Loaded;
                            agv.AgvRequest.Enabling_Mode = newAgv.Enabling_Mode;

                            if (!agv.AgvRequest.ModifyAllowed_Enabling_Mode)
                                agv.AgvRequest.SelectedEnabling_Mode = newAgv.Enabling_Mode;

                            agv.AgvRequest.SingleTrackAssigned = newAgv.SingleTrackAssigned;

                            agv.AgvRequest.AGV_NumPallet = newAgv.AGV_NumPallet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevels.Fatal);
                return;
            }
        }

        #endregion

        #region Global Events

        private void Global_OnEvery100mSec(object sender, GenericEventArgs e)
        {
            Now = DateTime.Now;

            // Aggiornamento stato
            //RefreshControllerStateAgv();

            //if (Global.Instance.Repetitions100mSec % 10 == 0)
            //{
            //    RefreshAgv();

            //    RefreshControllerStateStation();
            //}

            //if (Global.Instance.Repetitions100mSec % 19 == 0)
            //{
            //    Common.Instance.LoadMissions();
            //}
        }

        private void Global_OnEvery1Minute(object sender, GenericEventArgs e)
        {
            GC.Collect();
        }

        #endregion

        #region Private Methods

        private int _Repetitions = 0;

        private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_RefreshWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (_Repetitions == int.MaxValue)
                    _Repetitions = 0;
                else
                    _Repetitions++;

                // Aggiornamento stato
                if (_Repetitions % 10 == 0)
                {
                    RefreshControllerStateAgv();
                }

                if (_Repetitions % 20 == 0)
                {
                    RefreshAgv();

                    RefreshControllerStateStation();
                }

                if (_Repetitions % 50 == 0)
                {
                    Common.Instance.LoadMissions();
                }

                Thread.Sleep(100);
            }
        }

        private void RefreshControllerStateAgv()
        {
            var controllersList = Common.Instance.Agvs.Select(x => (int)x.AGV_CTR_Id).ToList();
            var controllersInfoList = Global.Instance.GetControllersInfo(controllersList);

            if (controllersInfoList == null || controllersInfoList.Count <= 0)
            {
                foreach (SEW_AGV agv in Common.Instance.Agvs)
                {
                    agv.NetworkState = EChannelStates.Unknown;
                }
            }
            else
            {
                foreach (SEW_AGV agv in Common.Instance.Agvs)
                {
                    var controllerInfo = controllersInfoList.FirstOrDefault(x => x.Id == agv.AGV_CTR_Id);

                    if (controllerInfo == null || controllerInfo.PublicProperties == null)
                    {
                        agv.NetworkState = EChannelStates.Unknown;
                        continue;
                    }

                    if (controllerInfo.NetworkState == (int)EChannelStates.Online)
                    {
                        if (controllerInfo.PublicProperties.Count() <= 0)
                        {
                            agv.NetworkState = EChannelStates.Connecting;
                        }
                        else
                        {
                            agv.NetworkState = EChannelStates.Online;
                            agv.InitFromPublicVariables(agv.PublicVariableFromGenericDTOs(controllerInfo.PublicProperties.ToArray()));
                        }
                    }
                    else
                    {
                        agv.NetworkState = EChannelStates.Offline;
                    }
                }
            }
        }

        private void RefreshControllerStateStation()
        {
            foreach (int ctr in AgvStationList.Select(a => a.MOD_CTR_Id).Distinct().Where(c => c != null))
            {
                var controller = Global.Instance.GetControllerInfo(ctr);
                if (controller != null && controller.PublicProperties != null)
                {
                    MemoryStream ms = new MemoryStream();
                    StreamWriter writer = new StreamWriter(ms);
                    foreach (string name in controller.PublicProperties.Select(g => g.Name))
                    {
                        AgvStation station = AgvStationList.FirstOrDefault(ags => ags.MOD_Code == name);
                        if (station != null)
                        {
                            ms.SetLength(0); // Per sicurezza ripulisco lo stream
                            ms.Position = 0;
                            writer.Write(controller.PublicProperties.Where(pp => pp.Name == name).FirstOrDefault().Value);
                            writer.Flush();
                            DataContractSerializer dcs = new DataContractSerializer(typeof(AgvStation));
                            ms.Position = 0;
                            AgvStation stationDTO = (AgvStation)dcs.ReadObject(ms);

                            station.Destination = stationDTO.Destination;
                            station.Status = stationDTO.Status;
                            station.Type = stationDTO.Type;
                            station.Udc = stationDTO.Udc;
                            station.PalletInCoda = stationDTO.PalletInCoda;
                        }
                    }
                    writer.Close();
                    ms.Close();
                }
            }
        }
        
        #endregion

        public void ShowScreen(RoutedPropertyChangedEventArgs<object> item)
        {
            TreeViewItem _itemSelected = (TreeViewItem)item.NewValue;
            // Sulle deselezioni non faccio niente
            if (_itemSelected == null) return;
            switch (_itemSelected.Name)
            {
                case "AGV":
                    ActivateItemAsync(AgvManagerViewModel);
                    break;
                case "Layout":
                    ActivateItemAsync(_agvLayoutViewModel);
                    MenuOpened = false;
                    break;
                case "Configuration":
                    ActivateItemAsync(CfgManagerViewModel);
                    break;
            }
        }
    }
    public class ControllerInfoState : BaseRuotineComponent
    {
        public ControllerInfoState(SqlConnection connection)
            : base(connection)
        {
        }
    }
}
