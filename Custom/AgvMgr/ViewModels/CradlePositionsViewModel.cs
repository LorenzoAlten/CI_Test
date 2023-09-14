using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using mSwAgilogDll.SEW;
using System;
using mSwAgilogDll;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace AgvMgr.ViewModels
{
    [Export(typeof(CradlePositionsViewModel))]
    public class CradlePositionsViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private List<HndModule> _cradleList;
        private HndModule _selectedCradle;

        #endregion

        #region Properties

        public SEW_AGV Agv { get; set; }

        public List<HndModule> CradleList
        {
            get { return _cradleList; }
            set
            {
                _cradleList = value;
                NotifyOfPropertyChange(() => CradleList);
            }
        }

        public HndModule SelectedCradle
        {
            get { return _selectedCradle; }
            set
            {
                _selectedCradle = value;
                NotifyOfPropertyChange(() => SelectedCradle);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public CradlePositionsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SEW_AGV agv)
        {
            Agv = agv;
            DisplayName = Agv.AGV_Code;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            CradleList = new List<HndModule>();
        }

        #endregion

        #region ViewModel Override

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            LoadStations();

            return base.OnActivateAsync(cancellationToken);
        }

        #endregion

        #region Private Methods

        private void LoadStations()
        {
            CradleList.Clear();
            {
                try
                {
                    CradleList = BaseBindableDbEntity.GetList<HndModule>(DbUtils.CloneConnection(Global.Instance.ConnGlobal), $@"WHERE [MOD_HMT_Code] = 'AGVCRADLE' AND [MOD_PLN_Code] = '{Agv.AGV_Code}'");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, LogLevels.Fatal);
                    return;
                }
            }
        }

        private void ReqTracking()
        {
            if (SelectedCradle == null) return;

            string path = Path.GetFullPath(@"PackDataViewer.exe");
            Process process;

            if (!Utils.IsProcessOpen(path, out process))
            {
                process = new Process();
                process.StartInfo.FileName = path;

                process.StartInfo.Arguments = $"1 {SelectedCradle.MOD_CTR_Id} {SelectedCradle.MOD_Code}";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
            }
            else
            {
                // Se il tracking è già aperto in riferimento ad una altra navetta lo chiudo e riapro
                if (!process.CloseMainWindow())
                {
                    process.Kill();
                }
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = $"1 {SelectedCradle.MOD_CTR_Id} {SelectedCradle.MOD_Code}";
                process.Start();
            }

        }

        #endregion

        #region Public methods

        public void LHD_01_01()
        {
            if (CradleList.Count < 1) return;

            SelectedCradle = CradleList[0];

            ReqTracking();
        }

        public void LHD_01_02()
        {
            if (CradleList.Count < 2) return;

            SelectedCradle = CradleList[1];

            ReqTracking();
        }

        public void LHD_02_01()
        {
            if (CradleList.Count < 3) return;

            SelectedCradle = CradleList[2];

            ReqTracking();
        }

        public void LHD_02_02()
        {
            if (CradleList.Count < 4) return;

            SelectedCradle = CradleList[3];

            ReqTracking();
        }

        #endregion
    }
}
