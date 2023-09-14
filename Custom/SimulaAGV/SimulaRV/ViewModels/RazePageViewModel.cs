using AgilogDll.EntitiesDepallettizer;
using AgilogDll.MFC.Telegrams;
using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimulaRV.ViewModels
{
    [Export(typeof(RazePageViewModel))]
    class RazePageViewModel : Screen, IHandle<bool>
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private int _TelLength;
        private int _BitOk;
        private EStatusRazeTel _StatusOK;

        private List<SimulaRaze_Ctr> _controllers;
        private List<HndPath> _paths;

        #endregion

        #region Properties

        public int TelLenght
        {
            get { return _TelLength; }
            set
            {
                _TelLength = value;
                NotifyOfPropertyChange(() => TelLenght);
            }
        }

        public int BitOk
        {
            get { return _BitOk; }
            set
            {
                _BitOk = value;
                NotifyOfPropertyChange(() => BitOk);
            }
        }

        public EStatusRazeTel StatusOk
        {
            get { return _StatusOK; }
            set
            {
                _StatusOK = value;
                NotifyOfPropertyChange(() => StatusOk);
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

        public List<CustomComboBoxItem> TelStatus { get; set; }

        public ObservableCollection<string> ReceivedTelegrams { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public RazePageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Raze");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;
        }

        #endregion

        #region ViewModel Override

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Init();
        }

        #endregion

        #region Public methods

        public async Task SendAsync()
        {
            IsLoading = true;

            await Task.Run(() =>
            {
                var controller = AppViewModel.Instance.SelectedController;

                Raze_Tel telegram = new Raze_Tel();
                telegram.TelLength = "000010";
                telegram.ResponseBit = BitOk;
                telegram.Status = StatusOk;


                string message = $"{telegram.TelLength}#{telegram.Status}#{telegram.ResponseBit}";

                controller.SendTelegram(message, "", false);
            });

            IsLoading = false;
        }

        public void Clean()
        {
            TelLenght = 0;
            BitOk = 0;
            StatusOk = EStatusRazeTel.OK;
        }

        #endregion

        #region Private methods

        protected void Init()
        {
            IsLoading = true;

            Task.Run(() =>
            {
                _paths = HndPath.GetList<HndPath>(Global.Instance.ConnGlobal);

                TelStatus = new List<CustomComboBoxItem>();
                foreach (string name in Enum.GetNames(typeof(EStatusRazeTel)))
                {
                    TelStatus.Add(new CustomComboBoxItem(name, Enum.Parse(typeof(EStatusRazeTel), name)));
                }

                ReceivedTelegrams = new ObservableCollection<string>();

            }).ContinueWith(antecedent =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotifyOfPropertyChange(() => TelStatus);
                });

                IsLoading = false;
            });
        }

        #endregion

        #region Global Events

        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
        }

        public void Handle(bool message)
        {
            // Il valore di message indica se l'inizializzazione
            // di tutti i componenti è andata a buon fine
            if (!message) return;

            try
            {
                _controllers = new List<SimulaRaze_Ctr>(AppViewModel.Instance.Managers.Where(m => m.ControllerCollection != null && m.ControllerCollection.Count > 0).
                                                                                      SelectMany(m => m.ControllerCollection).
                                                                                      Where(c => c is SimulaRaze_Ctr).
                                                                                      Cast<SimulaRaze_Ctr>());
                _controllers.ForEach(c => c.OnTelegramReceived += OnTelegramReceived);
            }
            catch { }
        }

        private void OnTelegramReceived(object sender, GenericEventArgs e)
        {
            SimulaRaze_Tel telegram = new SimulaRaze_Tel();
            telegram.ParseReceivedMessage(e.Arguments[1].ToString(), out var response);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ReceivedTelegrams != null)
                {
                    ReceivedTelegrams.Insert(0, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} - {e.Arguments[1].ToString()}");
                    if (ReceivedTelegrams.Count > 100) ReceivedTelegrams.RemoveAt(100);
                }
            });
        }
    }

    #endregion

}

