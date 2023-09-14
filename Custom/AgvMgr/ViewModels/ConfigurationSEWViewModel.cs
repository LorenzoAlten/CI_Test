using Caliburn.Micro;
using mSwDllWPFUtils;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using mSwDllUtils;
using AgvMgr.Entites;
using System.Threading.Tasks;
using System.Threading;

namespace AgvMgr.ViewModels
{
    [Export(typeof(CfgManagerViewModel))]
    public class ConfigurationSEWViewModel : Screen, IHandle<string>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _isReadOnly = true;
        private bool _isEnabledSave = false;
        private int _oldCtrCradleNumber;
        private int _lastControllerIdAgv;
        private int? _lastControllerIdCradle;
        private Visibility _simulatorVisible = Visibility.Visible;
        private Visibility _cradleVisible = Visibility.Visible;
        private Visibility _isAdmin = Visibility.Visible;
        private Visibility _combination = Visibility.Visible;
        private Visibility _combinationCradle = Visibility.Visible;
        private bool _isEnabled = true;
        private bool _isEnabledCancel = false;
        private bool _groupBoxEnabled = true;



        #endregion

        #region Properties

        public AgvEntities Agv { get; set; }
        public int CTR_ID_Agv { get; set; }
        public string CTR_Code_Agv { get; set; }
        public string Code_Agv_Machine { get; set; }
        public string CTR_MMG_Code { get; set; }
        public string CHL_IP_Agv { get; set; }
        public string CHL_Port_Agv { get; set; }
        public string CTR_Class_Agv { get; set; }


        public int CTR_ID_Cradle { get; set; }
        public string CTR_Code_Cradle { get; set; }
        public int CTR_Number_Cradle { get; set; }
        public string CHL_IP_Cradle { get; set; }
        public string CHL_Port_Cradle_S { get; set; }
        public string CTR_Class_Cradle { get; set; }
        public string CHL_Port_Cradle_R { get; set; }

        public bool Simulator { get; set; }
        public Visibility SimulatorVisible
        {
            get { return _simulatorVisible; }
            set
            {
                _simulatorVisible = value;
                NotifyOfPropertyChange(() => SimulatorVisible);
            }
        }

        public Visibility CradleVisible
        {
            get { return _cradleVisible; }
            set
            {
                _cradleVisible = value;
                NotifyOfPropertyChange(() => CradleVisible);
            }
        }

        public Visibility IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                NotifyOfPropertyChange(() => IsAdmin);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        public bool IsEnabledCancel
        {
            get { return _isEnabledCancel; }
            set
            {
                _isEnabledCancel = value;
                NotifyOfPropertyChange(() => IsEnabledCancel);
            }
        }

        public bool GroupBoxEnabled
        {
            get { return _groupBoxEnabled; }
            set
            {
                _groupBoxEnabled = value;
                NotifyOfPropertyChange(() => GroupBoxEnabled);
            }
        }

        public Visibility Combination
        {
            get { return _combination; }
            set
            {
                _combination = value;
                NotifyOfPropertyChange(() => Combination);
            }
        }

        public Visibility CombinationCradle
        {
            get { return _combinationCradle; }
            set
            {
                _combinationCradle = value;
                NotifyOfPropertyChange(() => CombinationCradle);
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                NotifyOfPropertyChange(() => IsReadOnly);
            }
        }



        public bool IsEnabledSave
        {
            get { return _isEnabledSave; }
            set
            {
                _isEnabledSave = value;
                NotifyOfPropertyChange(() => IsEnabledSave);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ConfigurationSEWViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, AgvEntities agv, bool editable)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            if (agv.CTR_ID != 0)
            {
                Agv = agv;
                //Agv.SimulatedTransponder = (ushort)Agv.AGV_CTR_Id;
                //NotifyOfPropertyChange(() => Agv.SimulatedTransponder);

                CTR_ID_Agv = agv.CTR_ID;
                CTR_Code_Agv = agv.CTR_Code;
                Code_Agv_Machine = agv.AGV_Code;
                CTR_MMG_Code = agv.CTR_MMG_Code;
                CHL_IP_Agv = agv.CHL_IP;
                CTR_Class_Agv = agv.CTR_Class;
                CHL_Port_Agv = agv.CHL_Port.ToString();

                string b = "";

                for (int i = 0; i < agv.CTR_Code.Length; i++)
                {
                    if (char.IsDigit(agv.CTR_Code[i]))
                        b += agv.CTR_Code[i];
                }

                if (b.Length > 0)
                    CTR_Number_Cradle = int.Parse(b);

                if (agv.CradleEntities.Count == 0)
                {
                    CradleVisible = Visibility.Collapsed;

                    if (IsAdmin == Visibility.Visible && CradleVisible == Visibility.Visible)
                    {
                        Combination = Visibility.Visible;
                    }
                    else
                    {
                        Combination = Visibility.Collapsed;
                    }
                }
                else
                {

                    foreach (var cradle in agv.CradleEntities)
                    {
                        CTR_ID_Cradle = cradle.CTR_ID;

                        b = "";

                        for (int i = 0; i < cradle.CTR_Code.Length; i++)
                        {
                            if (char.IsDigit(cradle.CTR_Code[i]))
                                b += cradle.CTR_Code[i];
                        }

                        if (b.Length > 0)
                            CTR_Number_Cradle = int.Parse(b);

                        _oldCtrCradleNumber = CTR_Number_Cradle;
                        CTR_Code_Cradle = cradle.CTR_Code;
                        CHL_IP_Cradle = cradle.CHL_IP;
                        CTR_Class_Cradle = cradle.CTR_Class;
                        if (cradle.CHL_Direction == "S")
                        {
                            CHL_Port_Cradle_S = cradle.CHL_Port.ToString();
                        }
                        else
                        {
                            CHL_Port_Cradle_R = cradle.CHL_Port.ToString();
                        }
                    }
                }

                SimulatorVisible = Visibility.Hidden;
            }

            string query = $@"SELECT TOP(1) CTR_Id, CTR_MMG_CODE
                                FROM MFC_CONTROLLERS 
                                WHERE CTR_Class = 'mSwAgilogDll.SEW.SEW_Agv_Ctr'
                                ORDER BY 1 DESC";

            var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal);

            if (dt.Rows.Count > 0)
            {
                _lastControllerIdAgv = dt.Rows[0].GetValueI("CTR_ID");
                CTR_MMG_Code = dt.Rows[0].GetValue("CTR_MMG_CODE");
            }

            query = $@"SELECT TOP(1) CTR_ID 
                        FROM MFC_CONTROLLERS 
                        WHERE CTR_Class = 'mSwAgilogDll.SEW.SEW_Agv_Cradle_Ctr'
                        ORDER BY 1 DESC";

            _lastControllerIdCradle = (int)DbUtils.ExecuteScalar(query, Global.Instance.ConnGlobal);

            IsReadOnly = editable;

            if (!editable)
            {
                Code_Agv_Machine = "NewAgv";
                IsEnabledSave = true;
                IsEnabledCancel = true;
                _eventAggregator.PublishOnUIThreadAsync($"EnableSave.NewAgv");
            }
        }

        #endregion

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (Code_Agv_Machine == "NewAgv")
            {
                IsEnabled = false;
            }

            if (Global.Instance.CheckUserPerm("XXXW"))
            {
                IsAdmin = Visibility.Visible;
                IsEnabled = true;
            }
            else if (Global.Instance.CheckUserPerm("XXWW"))
            {
                IsAdmin = Visibility.Collapsed;
                IsEnabled = true;
            }
            else
            {
                IsAdmin = Visibility.Collapsed;
                IsEnabled = false;
            }

            if (IsAdmin == Visibility.Visible && CradleVisible == Visibility.Visible)
            {
                Combination = Visibility.Visible;
                CombinationCradle = Visibility.Visible;
            }
            else if (IsAdmin == Visibility.Visible)
            {
                Combination = Visibility.Visible;
                CombinationCradle = Visibility.Collapsed;
            }
            else
            {
                Combination = Visibility.Collapsed;
                CombinationCradle = Visibility.Collapsed;
            }
        }

        #region Global Events

        public void Edit()
        {
            IsReadOnly = false;
            IsEnabledSave = true;
            IsEnabledCancel = true;

            _eventAggregator.PublishOnUIThreadAsync($"EnableSave.{Code_Agv_Machine}");
        }

        public void Save()
        {
            if (Agv != null)
            {
                try
                {
                    // Update dei canali
                    string newNumber = "";
                    if (CTR_Number_Cradle < 10)
                    {
                        newNumber = "0" + CTR_Number_Cradle;
                    }
                    else
                    {
                        newNumber = CTR_Number_Cradle.ToString();
                    }

                    string error = null;

                    var query = $@"
                        DECLARE @RC int
                        DECLARE @Error nvarchar(500)

                        EXECUTE @RC = [dbo].[msp_AGV_UpdateAgv] 
                                       {Code_Agv_Machine.SqlFormat()}
                                      ,{newNumber.SqlFormat()}
                                      ,{CTR_Number_Cradle.SqlFormat()}
                                      ,{CTR_Class_Agv.SqlFormat()}
                                      ,{CHL_IP_Agv.SqlFormat()}
                                      ,{CHL_Port_Agv.SqlFormat()}
                                      ,{CTR_Class_Cradle.SqlFormat()}
                                      ,{CHL_Port_Cradle_S.SqlFormat()}
                                      ,{CHL_IP_Cradle.SqlFormat()}
                                      ,{CHL_Port_Cradle_R.SqlFormat()}
                                      ,{CTR_ID_Agv.SqlFormat()}
                                      ,{CTR_ID_Cradle.SqlFormat()}
                                      ,{CTR_Code_Agv.SqlFormat()}
                                      ,{CTR_Code_Cradle.SqlFormat()}
                                      ,{Agv.Old_AGV_Code.SqlFormat()}
                                      ,{_oldCtrCradleNumber.SqlFormat()}
                                      ,@Error OUTPUT

                          SELECT @RC          AS 'RetVal'
                                ,@Error       AS 'Error'";

                    var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, true, out error);

                    if (dt != null && dt.Rows.Count > 0 && string.IsNullOrWhiteSpace(error))
                    {
                        Global.AlertAsync(_windowManager, "Per rendere effettive le modifiche devi riavviare il Traffic Manager");
                        _eventAggregator.PublishOnUIThreadAsync("Ricarica");
                    }
                    else
                    {
                        Global.ErrorAsync(_windowManager, $"Errore nell'inserimento: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Global.ErrorAsync(_windowManager, ex.Message);
                }
            }
            else
            {
                try
                {
                    // AGV
                    string newNumber = "";
                    if (CTR_Number_Cradle < 10)
                    {
                        newNumber = "0" + CTR_Number_Cradle;
                    }
                    else
                    {
                        newNumber = CTR_Number_Cradle.ToString();
                    }

                    var localPort = int.Parse(CHL_Port_Agv) - 1;
                    string error = null;

                    var query = $@"
                        DECLARE @RC int
                        DECLARE @Error nvarchar(500)

                        EXECUTE @RC = [dbo].[msp_AGV_CreateAgv] 
                           {Code_Agv_Machine.SqlFormat()}
                          ,{CTR_Number_Cradle.SqlFormat()}
                          ,{_lastControllerIdCradle.SqlFormat()}
                          ,{_lastControllerIdAgv.SqlFormat()}
                          ,{CTR_MMG_Code.SqlFormat()}
                          ,{newNumber.SqlFormat()}
                          ,{CTR_Class_Agv.SqlFormat()}
                          ,{CHL_IP_Agv.SqlFormat()}
                          ,{CHL_Port_Agv.SqlFormat()}
                          ,{localPort.SqlFormat()}
                          ,{CTR_Class_Cradle.SqlFormat()}
                          ,{CHL_Port_Cradle_S.SqlFormat()}
                          ,{CHL_IP_Cradle.SqlFormat()}
                          ,{CHL_Port_Cradle_R.SqlFormat()}
                          ,{Simulator.SqlFormat()}
                          ,@Error OUTPUT

                          SELECT @RC          AS 'RetVal'
                                ,@Error       AS 'Error'";

                    var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, true, out error);

                    if (dt != null && dt.Rows.Count > 0 && string.IsNullOrWhiteSpace(error))
                    {
                        Global.AlertAsync(_windowManager, "Inserimento effettuato correttamente");
                        _eventAggregator.PublishOnUIThreadAsync("Ricarica");
                    }
                    else
                    {
                        Global.ErrorAsync(_windowManager, $"Errore nell'inserimento: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Global.ErrorAsync(_windowManager, ex.Message);
                }
            }
        }

        public async void Delete()
        {
            var delete = (bool) await Global.ConfirmOrCancelAsync(_windowManager, "Sei sicuro di voler cancellare?");

            if (delete)
            {
                try
                {
                    string newNumber = "";
                    if (CTR_Number_Cradle < 10)
                    {
                        newNumber = "0" + CTR_Number_Cradle;
                    }
                    else
                    {
                        newNumber = CTR_Number_Cradle.ToString();
                    }

                    string error = "";

                    var query = $@"
                        DECLARE @RC int
                        DECLARE @Error nvarchar(500)

                        EXECUTE @RC = [dbo].[msp_AGV_DeleteAgv] 
                                               {Agv.AGV_Code.SqlFormat()}
                                              ,{newNumber.SqlFormat()}
                                              ,@Error OUTPUT

                          SELECT @RC          AS 'RetVal'
                                ,@Error       AS 'Error'";

                    var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, true, out error);

                    if (dt != null && dt.Rows.Count > 0 && string.IsNullOrWhiteSpace(error))
                    {
                        await Global.AlertAsync(_windowManager, "Cancellazione effettuata correttamente");
                        await _eventAggregator.PublishOnUIThreadAsync("Ricarica");
                    }
                    else
                    {
                        await Global.ErrorAsync(_windowManager, $"Errore nella cancellazione: {error}");
                    }
                }
                catch (Exception ex)
                {
                    await Global.ErrorAsync(_windowManager, ex.Message);
                }
            }
        }

        public void Cancel()
        {
            _eventAggregator.PublishOnUIThreadAsync("Ricarica");
        }

        #endregion

        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            var mess = message.Split('.');
            if (mess[0] == "EnableSave")
            {
                if (Code_Agv_Machine == "NewAgv")
                {
                    IsEnabledSave = true;
                    GroupBoxEnabled = true;
                    IsEnabledCancel = true;
                    IsEnabled = false;
                }
                else if (mess[1] != Code_Agv_Machine)
                {
                    IsEnabledSave = false;
                    IsEnabled = false;
                    GroupBoxEnabled = false;
                }
                else
                {
                    IsEnabled = false;
                }
            }

            await Task.CompletedTask;
        }
    }
}
