using AgilogDll;
using Caliburn.Micro;
using Caliburn.Micro.Validation;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllUtils;
using mSwDllWPFUtils;
using PickMgr.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace PickMgr.ViewModels
{
    [Export(typeof(PickViewModel))]
    public class PickViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private SqlConnection _conn;
        private string _SnackBarMessage;

        private int _bayNr;
        private int _controller;
        private bool _isLoading;
        private List<BayModule> _bayModules;
        private bool _refreshing;
        private DateTime _lastRefresh;
        private bool _animate = false;

        protected new List<AgilogDll.Entities.MisMission> _missions;
        private string _Test;



        #endregion

        #region Properties

        public string Test
        {
            get { return _Test; }
            set
            {
                _Test = value;
                NotifyOfPropertyChange(() => Test);
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
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

        public ObservableCollection<BayModule> BayModules { get; set; } = new ObservableCollection<BayModule>();

        #endregion

        #region Constructor

        [ImportingConstructor]
        public PickViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, int bayNr, int controller)
        {
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            DisplayName = "PICKING";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _bayNr = bayNr;
            _controller = controller;
            _refreshing = false;
            _lastRefresh = DateTime.MinValue;
            _missions = new List<AgilogDll.Entities.MisMission>();
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnTelegramManagement -= Global_OnTelegramManagement;
                Global.Instance.OnEvery1Sec -= Global_OnEvery1Sec;
                await Global.Instance.RemoveCallBackClientMfcAsync(_controller);
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (_controller > 0)
            {
                await Global.Instance.AddCallBackClientMfcAsync(_controller);
            }

            IsLoading = true;

            await LoadBayInfoAsync();

            Global.Instance.OnTelegramManagement += Global_OnTelegramManagement;
            Global.Instance.OnEvery1Sec += Global_OnEvery1Sec;

            Test = null;
            IsLoading = false;
        }

        #endregion

        #region Global Events

        private void Global_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            // Ogni n secondi aggiorno lo stato baia (non conterei sul re-invio telegrammi)
            if (DateTime.Now.Subtract(_lastRefresh).TotalSeconds < 3 || _refreshing)
            {
                return;
            }

            _refreshing = true;

            try
            {
                _bayModules.ForEach(async b => await b.Refresh());
            }
            finally
            {
                _refreshing = false;
                _lastRefresh = DateTime.Now;
            }
        }

        private async void Global_OnTelegramManagement(object sender, GenericEventArgs e)
        {
            try
            {
                string message = e.Arguments[1].ToString();

                Handling_Tel telegram = new Handling_Tel();
                telegram.ParseReceivedMessage(message, out mSwDllMFC.Telegram response);

                if (telegram.TelegramType != ETelegramTypes.LCAP && telegram.TelegramType != ETelegramTypes.LPRE) return;

                var bayModule = _bayModules.FirstOrDefault(b => b.Module == telegram.Position);
                if (bayModule == null) return;

                // Triggero aggiornamento immediato sul modulo
                await bayModule.Refresh();

                if (telegram.TelegramType == ETelegramTypes.LCAP)
                {
                    // Niente da fare
                }
                else if (telegram.TelegramType == ETelegramTypes.LPRE)
                {
                    // Niente da fare
                }
            }
            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Carico le informazioni sulla baia
        /// </summary>
        private async Task LoadBayInfoAsync()
        {
            if (_bayNr <= 0) return;

            _bayModules = new List<BayModule>();
            string error = null;

            await Task.Run(() =>
            {
                _bayModules = BayModule.GetList(_conn, _bayNr, out error);
                _bayModules.ForEach(b =>
                {
                    b.OnEnablingChanged += BayModule_OnEnablingChanged;
                    b.OnReentranceRequested += BayModule_OnReentranceRequested;
                    b.OnToBufferRequested += BayModule_OnToBufferRequested;
                    b.OnRemoveRequested += BayModule_OnRemoveRequested;
                });
            });

            if (_bayModules == null)
            {
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            OnUIThread(() =>
            {
                BayModules.Clear();
                _bayModules.ForEach(b => BayModules.Add(b));
            });
        }

        #endregion

        #region Events

        private async void BayModule_OnEnablingChanged(object sender, GenericEventArgs e)
        {
            var bayModule = (BayModule)sender;
            if (bayModule == null) return;

            var value = (bool)e.Argument;

            if (!await Global.ConfirmAsync(_windowManager, $"Vuoi davvero {(value ? "ABILITARE" : "DISABILITARE")} la piazzola?"))
            {
                bayModule.RevertEnabling();
                return;
            }

            // Salvo l'impostazione su Db
            await Task.Run(() =>
            {
                var query = $@"
                    UPDATE [HND_BAY_MODULES]
                    SET [BMO_Capacity] = {(value ? 1 : 0)}
                    WHERE [BMO_BAY_Num] = {bayModule.Bay.SqlFormat()}
                    AND [BMO_MOD_Code] = {bayModule.Module.SqlFormat()}";
                return DbUtils.ExecuteNonQuery(query, Global.Instance.ConnGlobal);
            });
        }

        private async void BayModule_OnReentranceRequested(object sender, GenericEventArgs e)
        {
            var bayModule = (BayModule)sender;            
            if (bayModule == null) return;
            

            if (!await Global.ConfirmAsync(_windowManager, $"Vuoi davvero forzare il rientro dell'UDC dalla piazzola?"))
            {
                return;
            }

            IsLoading = true;

            // Lancio la stored di generazione missione di rientro
            var error = await bayModule.AgvReentranceAsync();
            if (!string.IsNullOrWhiteSpace(error))
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            ////Inizio Modifica Antonio
            //// Invio il telegramma DATA
            //var mission = _missions.FirstOrDefault(m => m.MIS_UDC_Code == bayModule.Udc);

            //if (mission == null)
            //{
            //    IsLoading = false;
            //    return;
            //}

            //Handling_Tel tel = new Handling_Tel(ETelegramTypes.DATA, "WTC1", "PLC1");
            //tel.Position = bayModule.Module;            
            //tel.MissionID = bayModule.Mission.Value;
            //tel.UDC_Barcode = mission.MIS_UDC_Code;
            //tel.UDC_Type = mission.Udc.UdcTypePLC;
            //tel.Destination = mission.MIS_Dest_MOD_Code;

            //Global.Instance.SendTelegram(_controller, tel, false, 0, out error);     
            IsLoading = false;
            SnackBarMessage = "Richiesta inviata con successo";
            //Fine Modifica Antonio
        }

        private async void BayModule_OnToBufferRequested(object sender, GenericEventArgs e)
        {
            var bayModule = (BayModule)sender;
            if (bayModule == null) return;

            if (!await Global.ConfirmAsync(_windowManager, $"Vuoi davvero forzare il rientro dell'UDC verso il buffer?"))
            {
                return;
            }

            IsLoading = true;

            // Lancio la stored di generazione missione di rientro
            var error = await bayModule.AgvToBufferAsync();
            if (!string.IsNullOrWhiteSpace(error))
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            ////Inizio Modifica Antonio
            //// Invio il telegramma DATA
            //var mission = _missions.FirstOrDefault(m => m.MIS_UDC_Code == bayModule.Udc);

            //if (mission == null)
            //{
            //    IsLoading = false;
            //    return;
            //}

            //Handling_Tel tel = new Handling_Tel(ETelegramTypes.DATA, "WTC1", "PLC1");
            //tel.Position = bayModule.Module;            
            //tel.MissionID = bayModule.Mission.Value;
            //tel.UDC_Barcode = mission.MIS_UDC_Code;
            //tel.UDC_Type = mission.Udc.UdcTypePLC;
            //tel.Destination = mission.MIS_Dest_MOD_Code;

            //Global.Instance.SendTelegram(_controller, tel, false, 0, out error);     
            IsLoading = false;
            SnackBarMessage = "Richiesta inviata con successo";
            //Fine Modifica Antonio
        }

        private async void BayModule_OnRemoveRequested(object sender, GenericEventArgs e)
        {
            var bayModule = (BayModule)sender;
            if (bayModule == null) return;

            if (!await Global.ConfirmAsync(_windowManager, $"Vuoi davvero forzare il rientro dell'UDC dalla piazzola?"))
            {
                return;
            }

            IsLoading = true;

            // Cancello la missione
            var error = await bayModule.RemoveMissionAsync();
            if (!string.IsNullOrWhiteSpace(error))
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, error);
                return;
            }

            // Invio telegramma di cancellazione missione e posizione logica
            var lcdl = new Handling_Tel(ETelegramTypes.LCDL, "WCS", "PLC");
            lcdl.MissionID = bayModule.Mission.Value;

            var tkdl = new Handling_Tel(ETelegramTypes.TKDL, "WCS", "PLC");
            tkdl.Position = bayModule.Module;

            Global.Instance.SendTelegram(_controller, lcdl, false, 0, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, $"Impossibile inviare il telegramma di cancellazione missione al PLC: {error}");
                return;
            }

            Global.Instance.SendTelegram(_controller, tkdl, false, 0, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                IsLoading = false;
                await Global.ErrorAsync(_windowManager, $"Impossibile inviare il telegramma di cancellazione tracking al PLC: {error}");
                return;
            }

            IsLoading = false;
            SnackBarMessage = "Richiesta inviata con successo";
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
