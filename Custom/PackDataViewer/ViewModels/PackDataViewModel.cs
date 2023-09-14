using mSwAgilogDll;
using mSwAgilogDll.ViewModels;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using mSwAgilogDll.Errevi;
using System.Threading;
using System.Linq;

namespace PackDataViewer.ViewModels
{
    [Export(typeof(PackDataViewModel))]
    class PackDataViewModel : Conductor<Screen>
    {
        #region Members

        private bool TKDT_FirstReed = true;

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private RunUtils _utils;

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private bool _enabledForEditing;
        private bool _canDetails;
        private bool _canAbort;
        private bool _canEvacuate;
        private bool _canDelete;
        private bool _canSave;
        private string _udcCode;
        private int _udcType;
        private long _missionID;
        private string _destination;
        private bool _positionHasAlarm;
        private string _positionAlarm;
        private string _cradleCode;

        private string _INITIAL_udcCode;
        private string _FINAL_udcCode;
        private long _INITIAL_missionID;
        private long _FINAL_missionID;

        private int _INITIAL_udcType;
        private string _INITIAL_destination;

        private DateTime _requestTime;
        private int _requestTimeout = 30;
        private bool _requestOk;

        private DateTime _commandTime;
        private int _commandTimeout = 3;
        private bool _commandOk;
        private bool _missionAbortRequested;
        private AgilogDll.Entities.MisMission _mission;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);

                _eventAggregator.PublishOnUIThreadAsync(!value);
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

        public List<CustomComboBoxItem> UdcTypes { get; set; }

        public List<CustomComboBoxItem> Destinations { get; set; }

        public bool EnabledForEditing
        {
            get { return _enabledForEditing; }
            set
            {
                _enabledForEditing = value;
                NotifyOfPropertyChange(() => EnabledForEditing);
            }
        }

        public bool CanDetails
        {
            get { return _canDetails; }
            set
            {
                _canDetails = value;
                NotifyOfPropertyChange(() => CanDetails);
            }
        }

        public bool CanAbort
        {
            get { return _canAbort; }
            set
            {
                _canAbort = value;
                NotifyOfPropertyChange(() => CanAbort);
            }
        }

        public bool CanEvacuate
        {
            get { return _canEvacuate; }
            set
            {
                _canEvacuate = value;
                NotifyOfPropertyChange(() => CanEvacuate);
            }
        }

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                _canDelete = value;
                NotifyOfPropertyChange(() => CanDelete);
            }
        }

        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public string UdcCode
        {
            get { return _udcCode; }
            set
            {
                _udcCode = value;
                NotifyOfPropertyChange(() => UdcCode);
                LoadUdcAsync();
            }
        }

        public string CradleCode
        {
            get { return _cradleCode; }
            set
            {
                _cradleCode = value;
                NotifyOfPropertyChange(() => CradleCode);
            }
        }

        public int INITIAL_UdcType
        {
            get { return _INITIAL_udcType; }
            set
            {
                _INITIAL_udcType = value;
                NotifyOfPropertyChange(() => INITIAL_UdcType);
            }
        }

        public string INITIAL_Destination
        {
            get { return _INITIAL_destination; }
            set
            {
                _INITIAL_destination = value;
                NotifyOfPropertyChange(() => INITIAL_Destination);
            }
        }

        public string INITIAL_UdcCode
        {
            get { return _INITIAL_udcCode; }
            set
            {
                _INITIAL_udcCode = value;
                NotifyOfPropertyChange(() => INITIAL_UdcCode);
            }
        }



        public int UdcType
        {
            get { return _udcType; }
            set
            {
                _udcType = value;
                NotifyOfPropertyChange(() => UdcType);
            }
        }

        public long MissionID
        {
            get { return _missionID; }
            set
            {
                _missionID = value;
                NotifyOfPropertyChange(() => MissionID);
                LoadMission();
            }
        }

        public long INITIAL_MissionID
        {
            get { return _INITIAL_missionID; }
            set
            {
                _INITIAL_missionID = value;
                NotifyOfPropertyChange(() => INITIAL_MissionID);
            }
        }

        public long FINAL_MissionID
        {
            get { return _FINAL_missionID; }
            set
            {
                _FINAL_missionID = value;
                NotifyOfPropertyChange(() => FINAL_MissionID);
            }
        }

        public string FINAL_UdcCode
        {
            get { return _FINAL_udcCode; }
            set
            {
                _FINAL_udcCode = value;
                NotifyOfPropertyChange(() => FINAL_UdcCode);
            }
        }

        public string Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
        }

        public bool PositionHasAlarm
        {
            get { return _positionHasAlarm; }
            set
            {
                _positionHasAlarm = value;
                NotifyOfPropertyChange(() => PositionHasAlarm);
            }
        }

        public string PositionAlarm
        {
            get { return _positionAlarm; }
            set
            {
                _positionAlarm = value;
                NotifyOfPropertyChange(() => PositionAlarm);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public PackDataViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = null;

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            IsLoading = true;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;
            Global.Instance.OnTelegramManagement += Global_OnTelegramManagement;
            await Global.Instance.AddCallBackClientMfcAsync(Common.ControllerID);

            if (Common.ControllerID_TR > 0)
                await Global.Instance.AddCallBackClientMfcAsync(Common.ControllerID_TR);

            DisplayName = Common.Position;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            IsLoading = true;

            // Richiedo i dati di tracking
            UdcTypes = new List<CustomComboBoxItem>();
            Destinations = new List<CustomComboBoxItem>();

            await Task.Run(() =>
            {
                var query = $@"
                    SELECT [UPL_Num]
                          ,[UPL_UDT_Code] + ' Alt. ' + HGT_Desc + ', Peso ' + WGH_Desc 
                      FROM [UDC_CFG_TYPES_PLC]
                      INNER JOIN [dbo].[WHS_CFG_HEIGHT_CLASSES] ON HGT_Num = UPL_HGT_Num
                      INNER JOIN [dbo].[WHS_CFG_WEIGHT_CLASSES] ON WGH_Num = UPL_WGH_Num";
                var dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, true);
                foreach (DataRow row in dt.Rows)
                {
                    UdcTypes.Add(new CustomComboBoxItem(row.GetValue(1), row.GetValueI(0)));
                }

                query = $@"
                    SELECT DISTINCT Next_MOD_Code FROM [dbo].[mfn_HND_GetPaths] ({Common.Position.SqlFormat()}, Default)";
                dt = DbUtils.ExecuteDataTable(query, Global.Instance.ConnGlobal, true);
                Destinations.Add(new CustomComboBoxItem(Common.Position, Common.Position));
                foreach (DataRow row in dt.Rows)
                {
                    Destinations.Add(new CustomComboBoxItem(row.GetValue(0), row.GetValue(0)));
                }

                query = $@"SELECT [CTR_Code] FROM [MFC_CONTROLLERS] WHERE [CTR_Id] = {Common.ControllerID.SqlFormat()}";
                Common.ControllerIdentity = DbUtils.ExecuteScalar(query, Global.Instance.ConnGlobal).ToString();
            });

            NotifyOfPropertyChange(() => UdcTypes);
            NotifyOfPropertyChange(() => Destinations);

            Handling_Tel telegram = new Handling_Tel(ETelegramTypes.TKRD, Handling_Mgr.WcsIdentity, Common.ControllerIdentity);
            telegram.Position = Common.Position;
            CradleCode = Common.Position;
            var sep = '_';
            int count = telegram.Position.Count(s => s == sep);
            if (count > 1)
            {
                var newpos = telegram.Position.Substring(telegram.Position.IndexOf(sep) + 1);
                CradleCode = newpos;
                telegram.Position = newpos;
            }

            if (!Global.Instance.SendTelegram(Common.ControllerID, telegram, true, 0, out string error))
            {
                await Global.ErrorAsync(_windowManager, error);
                await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
            }
            _requestTime = DateTime.Now;
            _requestOk = false;
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
            Global.Instance.OnTelegramManagement -= Global_OnTelegramManagement;
            _eventAggregator.Unsubscribe(this);

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Public methods

        public async Task UdcDetailsAsync()
        {
            UdcsViewModel vm = new UdcsViewModel(_windowManager, _eventAggregator, UdcCode, EUdcPositions.Both, null, null, null, true);
            await _windowManager.ShowWindowAsync(vm);
        }

        public async Task AbortAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to abort the selected mission?")))
            {
                return;
            }

            IsLoading = true;
            string error = null;

            Handling_Tel handling = new Handling_Tel(ETelegramTypes.TKDL, Handling_Mgr.WcsIdentity, Common.ControllerIdentity);
            handling.Position = Common.Position;
            if (!Global.Instance.SendTelegram(Common.ControllerID, handling, true, 0, out error))
            {
                await Global.ErrorAsync(_windowManager, error);
                await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
                return;
            }

            _commandOk = true;
            _commandTime = DateTime.Now;
            _missionAbortRequested = true;
        }

        public async Task DeleteAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to delete current position tracking data?")))
            {
                return;
            }

            IsLoading = true;

            Handling_Tel telegram = new Handling_Tel(ETelegramTypes.TKDL, Handling_Mgr.WcsIdentity, Common.ControllerIdentity);
            telegram.Position = CradleCode;
            if (!Global.Instance.SendTelegram(Common.ControllerID, telegram, true, 0, out string error))
            {
                await Global.ErrorAsync(_windowManager, error);
                await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
            }
            _commandOk = true;
            _commandTime = DateTime.Now;
            await Global.AlertAsync(_windowManager, "Tracking cancellato con successo");
        }

        public async Task SaveAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to overwrite current position tracking data?")))
            {
                return;
            }

            IsLoading = true;

            /* ABILITAZIONE AL SALVATAGGIO DATI */
            bool AllowSave = CheckCanSave();

            /* Imposto messaggio di errore solo nel caso in cui AllowSave sia FALSE */
            string SAVE_Error = (AllowSave) ? string.Empty :
                ((CanSave)
                    ? ((!TKDT_FirstReed)
                        ? (((INITIAL_MissionID == FINAL_MissionID) && (INITIAL_UdcCode == FINAL_UdcCode))
                            ? ((INITIAL_UdcCode != UdcCode) || (INITIAL_UdcType != UdcType) || (INITIAL_Destination != Destination)
                                ? ((!string.IsNullOrWhiteSpace(UdcCode))
                                    ? string.Empty
                                    : Global.Instance.LangTl("Error: UDC Code is not valid!"))
                                : Global.Instance.LangTl("Error: UdcCode | UdcType | Destination not changed. Impossible to save data!"))
                            : Global.Instance.LangTl("Error: Initial and Actual tracking are different! Impossible to save data!"))
                        : Global.Instance.LangTl("Error: Save is not allowed before the first Tracking read happen!"))
                    : Global.Instance.LangTl("Error: Tracking Data changed while attempting to save! Impossible to save data!"));

            if ((AllowSave) && (string.IsNullOrWhiteSpace(SAVE_Error)))
            {
                Handling_Tel telegram = new Handling_Tel(ETelegramTypes.TKUP, Handling_Mgr.WcsIdentity, Common.ControllerIdentity);
                telegram.Position = CradleCode;
                telegram.MissionID = MissionID;
                telegram.UDC_Barcode = UdcCode;
                telegram.UDC_Type = UdcType;
                telegram.Destination = Destination;

                if (!Global.Instance.SendTelegram(Common.ControllerID, telegram, true, 0, out string error))
                {
                    await Global.ErrorAsync(_windowManager, error);
                    await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
                }
                _commandOk = true;
                _commandTime = DateTime.Now;
            }
            else
            {
                /* Questo caso avviene solo se CanSave diventa FALSE in maniera asincrona e SAVE viene premuto tra un aggiornamento e l'altro della proprietà di abilitazione pulsante */
                await Global.ErrorAsync(_windowManager, SAVE_Error);
                return;
            }
        }

        public async Task LoadUdcAsync()
        {
            IsLoading = true;
            CanDetails = false;
            CanAbort = false;
            CanEvacuate = false;
            CanDelete = false;
            CanSave = false;
            EnabledForEditing = false;
            PositionHasAlarm = false;

            UdcUdc udc = null;
            long misID = 0;
            AgilogDll.Entities.MisMission mission = null;
            string query;

            await Task.Run(() =>
            {
                query = $"SELECT [MIS_Id] FROM [MIS_MISSIONS] WHERE [MIS_UDC_Code] = {DbUtils.SqlFormat(_udcCode)}";
                var data = DbUtils.ExecuteScalar(query, Global.Instance.ConnGlobal);
                if (data == null || !(data is long))
                {
                    udc = null;
                    misID = 0;
                    return;
                }
                misID = (long)data;

                udc = new UdcUdc((SqlConnection)Global.Instance.ConnGlobal);
                if (!udc.GetByKey(_udcCode))
                {
                    udc = null;
                    misID = 0;
                }

                mission = new AgilogDll.Entities.MisMission((SqlConnection)Global.Instance.ConnGlobal);
                if (!mission.GetByKey(misID))
                {
                    mission = null;
                    misID = 0;
                }

                //LoadPositionAlarm();
            });

            IsLoading = false;

            if (udc == null)
            {
                await Global.AlertAsync(_windowManager, Global.Instance.LangTl("Load unit is incorrect"));
                return;
            }

            _missionID = misID;
            NotifyOfPropertyChange(() => MissionID);
            UdcType = udc.UdcTypePLC;

            Destination = mission?.MIS_Dest_MOD_Code;

            CanDetails = true;
            CanAbort = true;
            CanEvacuate = true;
            CanDelete = true;
            CanSave = true;
            EnabledForEditing = true;
        }

        public void LoadMission()
        {
            _mission = new AgilogDll.Entities.MisMission((SqlConnection)Global.Instance.ConnGlobal);
            if (!_mission.GetByKey(MissionID))
            {
                _mission = null;
            }
        }

        public async Task EvacuateAsync()
        {
            if (!await Global.ConfirmAsync(_windowManager, Global.Instance.LangTl("Do you really want to Evacuate the load unit?")))
                return;

            string udt = null;
            if (MissionID <= 0 && string.IsNullOrWhiteSpace(UdcCode))
            {
                var vm = new SelectElementViewModel(_windowManager, _eventAggregator);
                var retVal = await _windowManager.ShowDialogAsync(vm);

                if (!retVal.HasValue || !retVal.Value)
                    return;

                udt = vm.SelectedElement;
            }

            IsLoading = true;

            var row = await _utils.MisEvacAsync(MissionID > 0 ? (long?)MissionID : null, null, udt, Common.Position);
            if (row.GetValueI(0) != 0)
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, row.GetValue(1));
                return;
            }

            _commandOk = true;
            _commandTime = DateTime.Now;
        }

        #endregion

        #region Private methods

        private bool CheckCanSave()
        {
            /* Controllo ulteriormente che CanSave sia abilitato per evitare incongruenze con il metodo async che aggiorna il valore della proprietà */
            /* Se SAVE venisse premuto al momento dell'aggiornamento e i due metodi andassero ad eseguire contemporaneamente ma i dati non fossero giusti devo poter impedire la sovrascrittura dei dati sbagliati*/
            /* Controllo che sia avvenuto il primo aggiornamento di FINAL_MissionId e FINAL_UdcCode */
            bool temp = ((!TKDT_FirstReed)
                /* Controllo che i dati INITIAL e FINAL (MissionId e UdcCode) siano uguali tra loro */
                /* L'abilitazione al pulsante "CanSave" è già impostata per autoabilitarsi ogni secondo nel momento di ricezione di un TKDT */
                /* Eseguo comunque il controllo una volta aggiuntiva per evitare di salvare se i dati non sono corretti o nel caso CanSave non si fosse aggiornato a true */
                ? (((INITIAL_MissionID == FINAL_MissionID) && (INITIAL_UdcCode == FINAL_UdcCode))
                    ? ((INITIAL_UdcCode != UdcCode) || (INITIAL_UdcType != UdcType) || (INITIAL_Destination != Destination)
                        ? ((!string.IsNullOrWhiteSpace(UdcCode))
                            ? true
                            : false)
                        : false)
                    : false)
                : false);
            return temp;
        }

        private bool CheckCanDelete()
        {
            bool temp =
                     ((!TKDT_FirstReed)
                             ? (((INITIAL_MissionID == FINAL_MissionID) && (INITIAL_UdcCode == FINAL_UdcCode))
                                 ? ((FINAL_MissionID > 0)
                                     ? true
                                     : false)
                                 : false)
                             : false);
            return temp;
        }


        private async void LoadPositionAlarm()
        {
            await Task.Run(() =>
            {
                var query = $@"SELECT TOP 1 HAL_ALR_Desc FROM HAL WHERE HAL_ALR_Desc LIKE '%.{Common.Position}'
                               AND HAL_End_Date IS NULL
                               ORDER BY HAL_Start_Date DESC";
                var value = DbUtils.ExecuteScalar(query, Global.Instance.ConnGlobal);

                if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return;

                PositionHasAlarm = true;
                PositionAlarm = value.ToString();
            });
        }

        #endregion

        #region Global Events

        private async void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Verifico se è arrivata la risposta entro il timeout previsto
            if (!_requestOk &&
                DateTime.Now.Subtract(_requestTime).TotalSeconds >= _requestTimeout)
            {
                await Global.AlertAsync(_windowManager, Global.Instance.LangTl("No data received within the configured timeout"));
                await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
            }

            // Considero andato a buon fine un comando se non ricevo errori
            // entro il timeout configurato
            if (_commandOk &&
                DateTime.Now.Subtract(_commandTime).TotalSeconds >= _commandTimeout)
            {
                if (_missionAbortRequested)
                {
                    var row = _utils.MisAbort(MissionID);
                    if (row.GetValueI(0) != 0)
                    {
                        await Global.ErrorAsync(_windowManager, row.GetValue(1));
                        return;
                    }
                }

                await _eventAggregator.PublishOnUIThreadAsync(EAppViewCmds.Close);
            }
        }

        private async void Global_OnTelegramManagement(object sender, GenericEventArgs e)
        {
            try
            {
                int controller = (int)e.Arguments[0];
                string message = e.Arguments[1].ToString();

                if (controller == Common.ControllerID)
                {
                    var telegram = new Handling_Tel();
                    telegram.ParseReceivedMessage(message, out var response);

                    if (telegram.TelegramType != ETelegramTypes.TKDT &&
                        telegram.TelegramType != ETelegramTypes.TKER &&
                        telegram.TelegramType != ETelegramTypes.DREQ)
                    {
                        return;
                    }

                    // Ricezione dati di tracking
                    if (telegram.TelegramType == ETelegramTypes.TKDT &&
                        telegram.Position == CradleCode)
                    {
                        _requestOk = true;

                        if (TKDT_FirstReed)
                        {
                            INITIAL_MissionID = telegram.MissionID;
                            MissionID = telegram.MissionID;
                            FINAL_MissionID = telegram.MissionID;
                            if (MissionID > 0)
                            {
                                INITIAL_UdcCode = telegram.UDC_Barcode.Replace("\0", string.Empty).Trim().Replace("000000000000000000", string.Empty);
                                _udcCode = telegram.UDC_Barcode.Replace("\0", string.Empty).Trim().Replace("000000000000000000", string.Empty);
                                NotifyOfPropertyChange(() => UdcCode);
                                FINAL_UdcCode = telegram.UDC_Barcode.Replace("\0", string.Empty).Trim().Replace("000000000000000000", string.Empty);
                                UdcType = telegram.UDC_Type;
                                INITIAL_UdcType = telegram.UDC_Type;
                                Destination = telegram.Destination;
                                INITIAL_Destination = telegram.Destination;
                            }
                            else
                            {
                                UdcCode = string.Empty;
                                UdcType = 0;
                                Destination = string.Empty;
                            }
                            CanDetails = !string.IsNullOrWhiteSpace(UdcCode);
                            EnabledForEditing = !string.IsNullOrWhiteSpace(UdcCode);
                            CanDetails = !string.IsNullOrWhiteSpace(UdcCode);
                            CanAbort = _mission != null;
                            //CanEvacuate = _mission != null;
                            CanEvacuate = true; // Rendo l'evacuazione sempre attiva
                                                //CanDelete = !string.IsNullOrWhiteSpace(UdcCode);
                            CanDelete = true;
                            CanSave = !string.IsNullOrWhiteSpace(UdcCode);

                            //LoadPositionAlarm();

                            IsLoading = false;

                            TKDT_FirstReed = false;
                        }
                        else if (!TKDT_FirstReed)
                        {
                            FINAL_MissionID = telegram.MissionID;
                            if (FINAL_MissionID > 0)
                            {
                                _FINAL_udcCode = telegram.UDC_Barcode.Replace("\0", string.Empty).Trim().Replace("000000000000000000", string.Empty);
                                NotifyOfPropertyChange(() => FINAL_UdcCode);
                            }
                            //else
                            //{
                            //    UdcCode = string.Empty;
                            //}
                            IsLoading = false;
                        }

                        //CanSave = CheckCanSave();
                        CanDelete = CheckCanDelete();
                    }

                    // Errore di comando
                    if (telegram.TelegramType == ETelegramTypes.TKER &&
                        telegram.Position == CradleCode)
                    {
                        IsLoading = false;
                        _commandOk = false;

                        string error = null;
                        switch (telegram.TrackingErrorCode)
                        {
                            case ETrackingErrors.BAD_ZONE:
                                error = Global.Instance.LangTl("Zone status does not allow to perfom the requested command");
                                break;

                            case ETrackingErrors.BAD_POSITION:
                                error = Global.Instance.LangTl("Target position cannot accept the requested command");
                                break;

                            default:
                                error = Global.Instance.LangTl("Command refused by the controller");
                                break;
                        }

                        await Global.AlertAsync(_windowManager, error);
                    }

                    // Richiesta dati
                    if (telegram.TelegramType == ETelegramTypes.DREQ &&
                        telegram.Position == CradleCode)
                    {
                        // Se vengono richiesti dati significa che ho una presenza
                        // pertanto posso abilitare il pulsante di evacuazione anche
                        // in assenza di dati
                        CanEvacuate = true;
                    }
                }
                else if (controller == Common.ControllerID_TR && _mission != null)
                {
                    IsLoading = false;
                    _commandOk = false;

                    await Global.AlertAsync(_windowManager, Global.Instance.LangTl("Cannot abort a mission that has already been taken in charge by shuttles"));
                }
            }
            catch (Exception ex)
            {
                await Global.ErrorAsync(_windowManager, ex.Message);
            }
        }

        #endregion
    }
}
