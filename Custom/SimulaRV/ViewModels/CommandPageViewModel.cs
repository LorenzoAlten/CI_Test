using Caliburn.Micro;
using mSwAgilogDll;
using mSwAgilogDll.Errevi;
using mSwDllGrpcCommon;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimulaRV.ViewModels
{
    [Export(typeof(CommandPageViewModel))]
    class CommandPageViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _IsLoading = false;
        private string _SnackBarMessage;

        private string _manager;
        private string _command;

        #endregion

        #region Properties

        public string Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                NotifyOfPropertyChange(() => Manager);
            }
        }

        public string Command
        {
            get { return _command; }
            set
            {
                _command = value;
                NotifyOfPropertyChange(() => Command);
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

        public ObservableCollection<string> SentCommands { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public CommandPageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Commands");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            await InitAsync();
        }

        #endregion

        #region Public methods

        public async Task SendAsync()
        {
            IsLoading = true;

            var error = await Task.Run(() =>
            {
                var command = new SendCommandRequest
                {
                    ClientId = Global.Instance.APP_Id,
                    ManagerCode = _manager,
                    Command = _command,
                };
                try
                {
                    var retVal = Global.Instance.TelegramServiceClient.SendCommand(command);
                    return retVal.Retval;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            });

            if (!string.IsNullOrWhiteSpace(error))
            {
                await Global.ErrorAsync(_windowManager, error);
            }
            else
            {
                SentCommands.Add($"{DateTime.Now.ToString("HH:mm:ss")} --> {Command}");
            }

            IsLoading = false;
        }

        #endregion

        #region Private methods

        protected async Task InitAsync()
        {
            SentCommands = new ObservableCollection<string>();
        }

        #endregion

        #region Global Events



        #endregion
    }
}
