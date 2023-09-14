using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace Movitrans.ViewModels
{
    [Export(typeof(EnSavingCfgViewModel))]
    public class EnSavingCfgViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Properties

        public Movitrans_Mgr Mgr { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public EnSavingCfgViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, Movitrans_Mgr mgr)
        {
            Mgr = mgr;
            DisplayName = "Opzioni";
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region ViewModel Override

        #endregion

        #region Private Methods

        #endregion

        #region Public methods

        #endregion
    }
}
