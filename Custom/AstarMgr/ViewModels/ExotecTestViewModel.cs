using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mSwDllUtils;
using mSwDllWPFUtils;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using mSwAgilogDll.MFC.Astar;
using System.Windows;
using System.IO;
using System.Net.Http;
using mSwDllMFC;
using System.Threading;

namespace AstarMgr.ViewModels
{
    class ExotecTestViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private IEventAggregator eventAggregator;
        private SqlConnection _conn;
        private Dictionary<string, object> _windowSettings;
        private string _testMessage;
        protected AstarController _controller;
        private string _jsonTypeList;
        private ExotecMethod _choiceJsonType;
        private string _choiceUrlList;
        private string _stringJson;
        private string _choiceActionList;
        private string _return;
        private HttpClient _httpClient;
        private string _SnackBarMessage;

        #endregion

        #region Properties

        public string JsonType
        {
            get { return _jsonTypeList; }
            set
            {
                _jsonTypeList = value;
                Return = null;
                NotifyOfPropertyChange(() => Return);
                NotifyOfPropertyChange(() => JsonType);
            }
        }

        public ExotecMethod ChoiceJsonType
        {
            get { return _choiceJsonType; }
            set
            {
                _choiceJsonType = value;
                ChoiceUrl = _choiceJsonType.Url;
                JsonType = _choiceJsonType.Json;

                // Vado a prendere il file corrispondente
                string[] filename = Directory.GetFiles("../Bin/AstarServiceSamples/", JsonType + ".json");
                //LogInfo($"Trying to call {Path.GetFileNameWithoutExtension(filename[0])} server method from AstarServiceLibrary");

                if (filename.Length>0)
                {
                    using (StreamReader r = new StreamReader(Path.GetFullPath(filename[0])))
                    {
                        _stringJson = r.ReadToEnd();
                    }

                    NotifyOfPropertyChange(() => StringJson);
                }
                else
                {
                    _stringJson = "";
                    Return = null;
                }
                if (!string.IsNullOrWhiteSpace(_stringJson))
                    ChoiceAction = ERestOperation.POST.ToString();
            }
        }

        public string StringJson
        {
            get { return _stringJson; }
            set
            {
                _stringJson = value;
                NotifyOfPropertyChange(() => StringJson);

                Return = null;

                if (!string.IsNullOrWhiteSpace(_stringJson))
                    ChoiceAction = ERestOperation.POST.ToString();
                else
                    ChoiceAction = ERestOperation.GET.ToString();
            }
        }

        public string ChoiceUrl
        {
            get { return _choiceUrlList; }
            set
            {
                _choiceUrlList = value;
                Return = null;
                NotifyOfPropertyChange(() => Return);
                NotifyOfPropertyChange(() => ChoiceUrl);
            }
        }

        public string ChoiceAction
        {
            get { return _choiceActionList; }
            set
            {
                _choiceActionList = value;
                Return = null;
                NotifyOfPropertyChange(() => Return);
                NotifyOfPropertyChange(() => ChoiceAction);
            }
        }

        public string Return
        {
            get { return _return; }
            set
            {
                _return = value;
                NotifyOfPropertyChange(() => Return);
            }
        }

        public string SnackBarMessage
        {
            get { return _SnackBarMessage; }
            set
            {
                _SnackBarMessage = string.Empty;
                NotifyOfPropertyChange(() => SnackBarMessage);

                _SnackBarMessage = value;
                NotifyOfPropertyChange(() => SnackBarMessage);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ExotecTestViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            this._windowManager = windowManager;
            this.eventAggregator = eventAggregator;
        }

        #endregion

        #region ViewModel Override

        public List<ExotecMethod> JsonTypeList { get; set; }

        public ObservableCollection<TrafficController> ControllerCollection { get; set; } = new ObservableCollection<TrafficController>();

        public List<CustomComboBoxItem> ActionList { get; set; }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            _controller = (AstarController)AstarMgr.Shared.Managers[0].ControllerCollection.FirstOrDefault();

            // Carico tutti i metodi che mi servono
            var methods = new ExotecMethod();
            List<ExotecMethod> listJson = methods.GetExotecMethods();

            JsonTypeList = new List<ExotecMethod>();

            // Popolo la lista
            foreach (var json in listJson)
            {
                JsonTypeList.Add(json);
            }

            ActionList = new List<CustomComboBoxItem>();
            ActionList.Add(new CustomComboBoxItem(("POST"), ERestOperation.POST.ToString()));
            ActionList.Add(new CustomComboBoxItem(("GET"), ERestOperation.GET.ToString()));
            ActionList.Add(new CustomComboBoxItem(("DELETE"), ERestOperation.DELETE.ToString()));

            NotifyOfPropertyChange(() => ActionList);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!Global.Instance.AppClosing)
                Global.Instance.RefreshCurrentUser();

            await base.OnActivateAsync(cancellationToken);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            ChoiceUrl = "health";
            ChoiceAction = ERestOperation.GET.ToString();
        }

        #endregion

        #region Public methods

        public void Clear()
        {
            StringJson = null;
        }

        public async Task SendRequest()
        {
            object test = null;

            try
            {
                if (ChoiceAction == ERestOperation.GET.ToString())
                    test = await _controller.InvokeHostMethodAsync(ChoiceUrl, new object[] { ChoiceAction });
                else if (ChoiceAction == ERestOperation.POST.ToString())
                    test = await _controller.InvokeHostMethodAsync(ChoiceUrl, new object[] { ChoiceAction, StringJson });
                else
                    test = $"Invalid operation '{ChoiceAction}'";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (test == null)
            {
                test = Global.Instance.LangTl("An error occurred while trying to send the message");
            }

            //NotifyOfPropertyChange(() => Return);
            Return = test.ToString();
        }

        public string TestMessage
        {
            get { return _testMessage; }
            set
            {
                _testMessage = value;
                NotifyOfPropertyChange(() => TestMessage);
            }
        }

        private void LogInfo(string Message)
        {
            TestMessage += Message + "\n";
            //txtInfo.Dispatcher.BeginInvoke(new System.Action(() => txtInfo.Text += Message + "\n"), System.Windows.Threading.DispatcherPriority.Send);
            //txtInfo.Dispatcher.BeginInvoke(new Action(() => txtInfo.ScrollToEnd()), System.Windows.Threading.DispatcherPriority.Send);
        }

        public void ReturnCopy()
        {
            if (string.IsNullOrWhiteSpace(Return)) return;

            Clipboard.SetText(Return);
            SnackBarMessage = "Message copied in clipboard";
        }

        #endregion

        #region Global Events

        //private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        //{
        //    //// Ogni 5 secondi rinfresco i messaggi
        //    //if ((Global.Instance.Repetitions1Sec % 5) != 0) return;

        //    //RefreshMessages();

        //    // Ogni 30 secondi rinfresco i log
        //    if ((Global.Instance.Repetitions1Sec % 30) != 0) return;

        //    RefreshLogs();
        //}

        #endregion
    }
}
