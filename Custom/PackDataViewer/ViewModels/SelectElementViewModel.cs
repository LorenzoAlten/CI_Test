using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Threading;

namespace PackDataViewer.ViewModels
{
    [Export(typeof(SelectElementViewModel))]
    public class SelectElementViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private object _lockObj = new object();
        private CustomComboBoxItem _selectedItem;

        private bool _IsLoading = false;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public bool CanConfirm
        {
            get { return SelectedItem != null; }
        }

        public CustomComboBoxItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (value == null)
                    SelectedElement = null;
                else
                    SelectedElement = SelectedItem.Value.ToString();

                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(() => SelectedElement);
                NotifyOfPropertyChange(() => CanConfirm);
            }
        }

        public string SelectedElement { get; protected set; }

        /// <summary>
        /// Lista delle linee di produzione
        /// </summary>
        public ObservableCollection<CustomComboBoxItem> ElementsList { get; set; } = new ObservableCollection<CustomComboBoxItem>();

        #endregion

        #region Constructor

        [ImportingConstructor]
        public SelectElementViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = string.Empty;

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            BindingOperations.EnableCollectionSynchronization(ElementsList, _lockObj);
        }

        #endregion

        #region Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            await LoadListAsync();
        }

        #endregion

        #region Public Methods

        public async Task ConfirmAsync()
        {
            await TryCloseAsync(true);
        }

        public async Task CancelAsync()
        {
            SelectedItem = null;

            await TryCloseAsync(false);
        }

        #endregion

        #region Private methods

        private async Task LoadListAsync()
        {
            try
            {
                var list = UdcCfgType.GetList<UdcCfgType>(Global.Instance.ConnGlobal);
                if (list == null || list.Count <= 0)
                {
                    await Global.ErrorAsync(_windowManager, Global.Instance.LangTl("Cannot retrieve load unit types"));
                    return;
                }

                ElementsList = new ObservableCollection<CustomComboBoxItem>();
                list.ForEach(e => ElementsList.Add(new CustomComboBoxItem(e.UDT_Desc, e.UDT_Code)));

                SelectedItem = ElementsList[0];
            }
            catch (Exception) { }
        }

        #endregion
    }
}
