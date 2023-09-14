using mSwAgilogDll;
using Caliburn.Micro;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace OrdersMgr.ViewModels
{
    [Export(typeof(FilterOrderViewModel))]
    class FilterOrderViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private string _OrderCode = null;
        private string _OrderDescription = null;
        private DateTime? _FromCreationDate;
        private DateTime? _ToCreationDate;
        private DateTime? _FromDueDate;
        private DateTime? _ToDueDate;
        private string _MissionType = null;
        private int? _Priority;
        private string _Phase;
        private List<MisCfgType> _MisCfgTypeList;
        private List<MisCfgOrdPhase> _MisCfgOrdPhaseList;

        #endregion

        #region Properties

        /// <summary>
        /// Codice ordine
        /// </summary>
        public string OrderCode
        {
            get { return _OrderCode; }
            set
            {
                _OrderCode = value;
                NotifyOfPropertyChange(() => OrderCode);
            }
        }

        /// <summary>
        /// Descrizione ordine
        /// </summary>
        public string OrderDescription
        {
            get { return _OrderDescription; }
            set
            {
                _OrderDescription = value;
                NotifyOfPropertyChange(() => OrderDescription);
            }
        }

        /// <summary>
        /// Data creazione (dalla data)
        /// </summary>
        public DateTime? FromCreationDate
        {
            get { return _FromCreationDate; }
            set
            {
                _FromCreationDate = value;
                NotifyOfPropertyChange(() => FromCreationDate);
            }
        }

        /// <summary>
        /// Data creazione (alla data)
        /// </summary>
        public DateTime? ToCreationDate
        {
            get { return _ToCreationDate; }
            set
            {
                _ToCreationDate = value;
                NotifyOfPropertyChange(() => ToCreationDate);
            }
        }

        /// <summary>
        /// Data fine evasione (dalla data)
        /// </summary>
        public DateTime? FromDueDate
        {
            get { return _FromDueDate; }
            set
            {
                _FromDueDate = value;
                NotifyOfPropertyChange(() => FromDueDate);
            }
        }

        /// <summary>
        /// Data fine evasione (alla data)
        /// </summary>
        public DateTime? ToDueDate
        {
            get { return _ToDueDate; }
            set
            {
                _ToDueDate = value;
                NotifyOfPropertyChange(() => ToDueDate);
            }
        }

        /// <summary>
        /// Tipo operazione (Entrata, Inventario, Prelievo, Prelievo per Spedizione, Versamento)
        /// </summary>
        public string MissionType
        {
            get { return _MissionType; }
            set
            {
                _MissionType = value;
                NotifyOfPropertyChange(() => MissionType);
            }
        }

        /// <summary>
        /// Priorità
        /// </summary>
        public int? Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
                NotifyOfPropertyChange(() => Priority);
            }
        }

        /// <summary>
        /// Fase ordine
        /// </summary>
        public string Phase
        {
            get { return _Phase; }
            set
            {
                _Phase = value;
                NotifyOfPropertyChange(() => Phase);
            }
        }

        /// <summary>
        /// Lista tipi operazione
        /// </summary>
        public List<MisCfgType> MisCfgTypeList
        {
            get { return _MisCfgTypeList; }
            set
            {
                _MisCfgTypeList = value;
                NotifyOfPropertyChange(() => MisCfgTypeList);
            }
        }

        /// <summary>
        /// Lista delle priorità accettate
        /// </summary>
        public IEnumerable<int> PriorityList
        {
            get { return Enumerable.Range(0, 11); }
        }

        /// <summary>
        /// Lista fasi ordine
        /// </summary>
        public List<MisCfgOrdPhase> MisCfgOrdPhaseList
        {
            get { return _MisCfgOrdPhaseList; }
            set
            {
                _MisCfgOrdPhaseList = value;
                NotifyOfPropertyChange(() => MisCfgOrdPhaseList);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FilterOrderViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region ViewModel Override

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            MisCfgTypeList = BaseBindableDbEntity.GetList<MisCfgType>(Global.Instance.ConnGlobal);
            MisCfgOrdPhaseList = BaseBindableDbEntity.GetList<MisCfgOrdPhase>(Global.Instance.ConnGlobal);

            await base.OnInitializeAsync(cancellationToken);
        }

        #endregion

        #region Public methods

        public async Task ApplyAsync()
        {
            await _eventAggregator.PublishOnUIThreadAsync("APPLY_FILTER");
        }

        public async Task ClearAsync()
        {
            OrderCode = null;
            OrderDescription = null;
            FromCreationDate = null;
            ToCreationDate = null;
            FromDueDate = null;
            ToDueDate = null;
            MissionType = null;
            Priority = null;
            Phase = null;

            await _eventAggregator.PublishOnUIThreadAsync("CLEAR_FILTER");
        }

        #endregion
    }
}
