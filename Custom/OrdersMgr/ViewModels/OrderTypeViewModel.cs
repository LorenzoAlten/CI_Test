using Caliburn.Micro;
using mSwDllWPFUtils;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(OrderTypeViewModel))]
    public class OrderTypeViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        public string RetVal { get; private set; }

        #region Constructor

        [ImportingConstructor]
        public OrderTypeViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            DisplayName = Global.Instance.LangTl("Choose the order type");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
        }

        #endregion

        public async Task ChooseOrderTypeAsync(string type)
        {
            RetVal = type;

            await TryCloseAsync();
        }
    }
}
