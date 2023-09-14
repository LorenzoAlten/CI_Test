using Caliburn.Micro;
using mSwDllWPFUtils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using mSwAgilogDll.SEW;
using System.Collections.Generic;
using System.IO;
using mSwDllUtils;
using System.Diagnostics;
using System.Threading;

namespace AgvMgr.ViewModels
{
    [Export(typeof(SEWAgvViewModel))]
    public class SEWAgvViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _MoreDataExpanded;

        #endregion

        #region Properties

        public SEW_AGV Agv { get; set; }

        public bool MoreDataExpanded
        {
            get { return _MoreDataExpanded; }
            set
            {
                _MoreDataExpanded = value;
                NotifyOfPropertyChange(() => MoreDataExpanded);
            }
        }
        #endregion

        #region Constructor

        [ImportingConstructor]
        public SEWAgvViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SEW_AGV agv)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            Agv = agv;

            if (Agv != null)
            {
                Agv.SimulatedTransponder = Agv.Transponder;
                NotifyOfPropertyChange(() => Agv.SimulatedTransponder);
            }
        }

        #endregion

        #region Pulsanti
        public void EditMission()
        {
            _windowManager.ShowDialogAsync(new MissionDetailsViewModel(_windowManager, _eventAggregator, Agv));
        }

        public void Alarm()
        {
            string path = Path.GetFullPath(@"AlarmsViewer.exe");
            Process process;
            if (!Utils.IsProcessOpen(path, out process))
            {
                process = new Process();
                process.StartInfo.FileName = path;

                process.StartInfo.Arguments = "2 AGV AGVSEW " + Agv.AGV_Code;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

            }
            else
            {
                //-------------------------------------------------------------------------------------------
                // Se il log allarmi è già aperto in riferimento ad una altra navetta lo chiudo e riapro
                //-------------------------------------------------------------------------------------------
                if (!process.CloseMainWindow())
                {
                    process.Kill();
                }
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = "2 AGV AGVSEW " + Agv.AGV_Code;
                process.Start();
            }
        }

        public void AdditionalData()
        {
            _windowManager.ShowWindowAsync(new AdditionalViewModel(_windowManager, _eventAggregator, Agv));
        }

        public void OpenCradles()
        {
            _windowManager.ShowDialogAsync(new CradlePositionsViewModel(_windowManager, _eventAggregator, Agv));
        }

        public void SetSingleTrack()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                  "SINGLETRACK",
                                 new Dictionary<string, object>()
                                 {
                                     ["AGV_Code"] = Agv.AGV_Code
                                 },
                                  out string error))
                Global.ErrorAsync(_windowManager, error);

            //SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
            //{
            //    Service = SEW_Agv_Service.NO_SERVICE,
            //    Part_data = 0,
            //    UDP_receive_port = 0,
            //    Index = 0,
            //    Program_number = 0,
            //    Commands = SEW_Commands.Single_track
            //};

            //Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
        }
        public void Simula()
        {
            if (Agv.Agv_State.HasFlag(SEW_State_Flags.Simulation))
            {
                SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
                {
                    Part_data = 0,
                    UDP_receive_port = 0,
                    Index = 0,
                    Program_number = 0,
                    Set_transponder = Agv.SimulatedTransponder,
                    Commands = SEW_Commands.Continue
                };

                Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
            }
        }

        public void SetEnablingMode()
        {
            if (!Agv.AgvRequest.ModifyAllowed_Enabling_Mode) return;
            if (Agv.AgvRequest.Enabling_Mode == Agv.AgvRequest.SelectedEnabling_Mode) return;

            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                             "ENABLING_MODE",
                                            new Dictionary<string, object>()
                                            {
                                                ["Enabling_Mode"] = (int)Agv.AgvRequest.SelectedEnabling_Mode,
                                                ["AGV_Code"] = Agv.AGV_Code
                                            },
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }
        public void Stop()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                             "STOP",
                                            new Dictionary<string, object>()
                                            {
                                                ["AGV_Code"] = Agv.AGV_Code
                                            },
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }
        public void Start()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                             "START",
                                            new Dictionary<string, object>()
                                            {
                                                ["AGV_Code"] = Agv.AGV_Code
                                            },
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }
        public void LoadUnload()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                              "LOADUNLOAD",
                                             new Dictionary<string, object>()
                                             {
                                                 ["AGV_Code"] = Agv.AGV_Code
                                             },
                                              out string error))
                Global.ErrorAsync(_windowManager, error);
        }
        public void Continue()
        {
            if (!Global.Instance.SendCommandTrafficManager(Agv.MMG_Code,
                                              "CONTINUE",
                                             new Dictionary<string, object>()
                                             {
                                                 ["AGV_Code"] = Agv.AGV_Code
                                             },
                                              out string error))
                Global.ErrorAsync(_windowManager, error);

            //SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel();

            //cmd_telegram.Service = SEW_Agv_Service.NO_SERVICE;
            //cmd_telegram.Part_data = 0;
            //cmd_telegram.UDP_receive_port = 0;
            //cmd_telegram.Index = 0;
            //cmd_telegram.Program_number = 0;
            //cmd_telegram.Commands = SEW_Commands.Continue;

            //Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
        }
        public void AlarmReset()
        {
            SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
            {
                Service = SEW_Agv_Service.NO_SERVICE,
                Part_data = 0,
                UDP_receive_port = 0,
                Index = 0,
                Program_number = 0,
                Commands = SEW_Commands.Error_reset
            };

            Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
        }
        public void SetPartData()
        {
            SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
            {
                Service = SEW_Agv_Service.NO_SERVICE,
                Part_data = Agv.Part_data_To_send,
                UDP_receive_port = 0,
                Index = 0,
                Program_number = 0
            };

            Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
        }

        #endregion

        #region Global Events

        #endregion

        #region Private Methods

        #endregion
    }
}
