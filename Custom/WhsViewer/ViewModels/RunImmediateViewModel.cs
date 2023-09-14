using mSwAgilogDll;
using Caliburn.Micro;
using Microsoft.Win32;
using mSwDllUtils;
using mSwDllUtils.AdHocInstances;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Common;
using System.Management;

namespace WhsViewer.ViewModels
{
    [Export(typeof(RunImmediateViewModel))]
    public class RunImmediateViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private AgilogDll.RunUtils _utils;

        private AgilogDll.UdcUdc _Udc;
        string _udcCode;
        int _udcLength;
        string _itemCode;
        int? _owner;
        string _itemDesc;
        int? _bay;
        string _causal;
        string _notes;
        EImmediateOperations? _operation;
        decimal _requestedQuantity;
        bool _loadingIsVisible;
        bool _operationIsValid;
        bool _udcReadonly;
        bool _itemReadonly;
        List<CustomComboBoxItem> _bays;
        List<CustomComboBoxItem> _causals;
        OperationWorker _init;

        #endregion

        #region Properties

        public AgilogDll.UdcUdc Udc
        {
            get { return _Udc; }
            set
            {
                _Udc = value;
                NotifyOfPropertyChange(() => Udc);
            }
        }

        public bool LoadingIsVisible
        {
            get { return _loadingIsVisible; }
            set
            {
                _loadingIsVisible = value;
                NotifyOfPropertyChange(() => LoadingIsVisible);
            }
        }

        public bool OperationIsValid
        {
            get { return _operationIsValid; }
            set
            {
                _operationIsValid = value;
                NotifyOfPropertyChange(() => OperationIsValid);
            }
        }

        public bool UdcReadonly
        {
            get { return _udcReadonly; }
            set
            {
                _udcReadonly = value;
                NotifyOfPropertyChange(() => UdcReadonly);
            }
        }

        public bool ItemReadonly
        {
            get { return _itemReadonly; }
            set
            {
                _itemReadonly = value;
                NotifyOfPropertyChange(() => ItemReadonly);
            }
        }

        public string UDC_Code
        {
            get { return _udcCode; }
            set
            {
                _udcCode = value;
                NotifyOfPropertyChange(() => UDC_Code);

                LoadUdcFromDb(_udcCode);
            }
        }

        public int UdcLength
        {
            get { return _udcLength; }
            set
            {
                _udcLength = value;
                NotifyOfPropertyChange(() => UdcLength);

                RefreshOperationIsValid();
            }
        }

        public string ITM_Code
        {
            get { return _itemCode; }
            set
            {
                _itemCode = value;
                NotifyOfPropertyChange(() => ITM_Code);

                RefreshOperationIsValid();
            }
        }

        public int? Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                NotifyOfPropertyChange(() => Owner);

                RefreshOperationIsValid();
            }
        }

        public string ITM_Desc
        {
            get { return _itemDesc; }
            set
            {
                _itemDesc = value;
                NotifyOfPropertyChange(() => ITM_Desc);

                RefreshOperationIsValid();
            }
        }

        public int? BAY_Num
        {
            get { return _bay; }
            set
            {
                _bay = value;
                NotifyOfPropertyChange(() => BAY_Num);

                RefreshOperationIsValid();
            }
        }

        public bool SelectBay { get { return !BAY_Num.HasValue || BAY_Num.Value <= 0; } }

        public string Causal
        {
            get { return _causal; }
            set
            {
                _causal = value;
                NotifyOfPropertyChange(() => Causal);

                LoadBays();

                RefreshOperationIsValid();
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public decimal RequestedQuantity
        {
            get { return _requestedQuantity; }
            set
            {
                _requestedQuantity = value;
                NotifyOfPropertyChange(() => RequestedQuantity);

                RefreshOperationIsValid();
            }
        }

        public List<HndBayBay> HndBayBays { get; private set; }

        public List<CustomComboBoxItem> Bays
        {
            get { return _bays; }
            set
            {
                _bays = value;
                NotifyOfPropertyChange(() => Bays);
            }
        }

        public List<CustomComboBoxItem> Causals
        {
            get { return _causals; }
            set
            {
                _causals = value;
                NotifyOfPropertyChange(() => Causals);
            }
        }

        public List<AgilogDll.Item> Items { get; set; }
        public Dictionary<object, string> Dictionary_ItemCodes { get; set; }
        public Dictionary<object, string> Dictionary_ItemDescriptions { get; set; }
        public Dictionary<object, string> Dictionary_Udcs { get; set; }

        #endregion

        [ImportingConstructor]
        public RunImmediateViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
            EImmediateOperations? Operation = null,
            string Udc = null,
            string ItemCode = null,
            int? owner = null,
            int? Bay = null,
            decimal quantity = 0)
        {
            this.DisplayName = Global.Instance.LangTl("Immediate");

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _utils = new AgilogDll.RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));
            _operation = Operation;
            UDC_Code = Udc;
            UdcReadonly = Udc != null;
            ITM_Code = ItemCode;
            Owner = owner;
            ItemReadonly = ItemCode != null;
            BAY_Num = Bay;
            OperationIsValid = false;
            RequestedQuantity = quantity;

            _init = new OperationWorker(InitData);
            _init.OperationCompleted += _init_OperationCompleted;

            Init();

            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        #region Public Methods

        #endregion

        #region Private Methods

        private void Init()
        {
            LoadingIsVisible = true;

            _init.DoOperation();
        }

        private void InitData(object param, ExtBackgroundWorker worker)
        {
            ParamManager.Init(Global.Instance.ConnGlobal);

            // Se ho un solo articolo carico solo quello per risparimiare tempo
            if (ItemReadonly)
            {
                var item = new AgilogDll.Item((SqlConnection)Global.Instance.ConnGlobal);

                Dictionary<string, object> KeyValueFields =new Dictionary<string, object>();
                KeyValueFields.Add("ITM_CODE", _itemCode );
                KeyValueFields.Add("ITM_OWNER", _owner);
                item.GetByKey(KeyValueFields);
                Items = new List<AgilogDll.Item> { item };
            }
            else
            {
                Items = BaseBindableDbEntity.GetList<AgilogDll.Item>(Global.Instance.ConnGlobal);
            }

            Dictionary_ItemCodes = Items.ToDictionary(i => (object)i.ITM_Code, i => i.ITM_Code);
            Dictionary_ItemDescriptions = Items.ToDictionary(i => (object)i.ITM_Code, i => i.ITM_Desc);

            //var udcs = BaseBindableDbEntity.GetList<UdcUdc>(Global.Instance.ConnGlobal);
            //Dictionary_Udcs = udcs.ToDictionary(u => (object)u.UDC_Code, i => i.UDC_Code);
            UdcLength = ParamManager.GetValueI("APP_UDC_MinLength");

            string condition = null;

            if (_operation.HasValue)
            {
                condition = $@"INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
                               WHERE (([BYT_In] = 1 AND {_operation.Value.ToString().SqlFormat()} = 'Drop')
                               OR ([BYT_Out] = 1 AND {_operation.Value.ToString().SqlFormat()} <> 'Drop'))
                               AND [BAY_Immediate] = 1";
            }

            HndBayBays = BaseBindableDbEntity.GetList<HndBayBay>(Global.Instance.ConnGlobal, condition);
            if (HndBayBays != null)
            {
                Bays = HndBayBays.OrderBy(b => b.BAY_Order).Select(b => new CustomComboBoxItem(b.BAY_Desc, b.BAY_Num)).ToList();
            }
            if (BAY_Num.HasValue && !HndBayBays.Any(b => b.BAY_Num == BAY_Num.Value))
            {
                BAY_Num = null;
            }
            else if (HndBayBays.Count == 1)
            {
                BAY_Num = HndBayBays.First().BAY_Num;
            }

            //if (_operation.HasValue)
            //{
            //    condition = $@"INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
            //                   WHERE (([BYT_In] = 1 AND {_operation.Value.ToString().SqlFormat()} = 'Drop')
            //                   OR ([BYT_Out] = 1 AND {_operation.Value.ToString().SqlFormat()} <> 'Drop'))
            //                   AND [BAY_Immediate] = 1";
            //}

            //var causals = BaseBindableDbEntity.GetList<HndBayBay>(Global.Instance.ConnGlobal, condition);
            //if (causals != null)
            //{
            //    Bays = causals.OrderBy(b => b.BAY_Order).Select(b => new CustomComboBoxItem(b.BAY_BYT_Code, b.BAY_Desc)).ToList();
            //}
            //if (BAY_Num.HasValue && !causals.Any(b => b.BAY_Num == BAY_Num.Value))
            //{
            //    BAY_Num = null;
            //}
            //else if (causals.Count == 1)
            //{
            //    BAY_Num = causals.First().BAY_Num;
            //}





            //string condition = null;
            //if (_operation.HasValue)
            //{
            //    condition = $@"WHERE (([CAU_In] = 1 AND {_operation.Value.ToString().SqlFormat()} = 'Drop')
            //                   OR (([CAU_Out] = 1 OR [CAU_Pick] = 1) AND {_operation.Value.ToString().SqlFormat()} = 'Pick')
            //                   OR (([CAU_Out] = 1) AND {_operation.Value.ToString().SqlFormat()} = 'Ship')
            //                   OR ([CAU_Inventory] = 1 AND {_operation.Value.ToString().SqlFormat()} = 'Inventory'))
            //                   AND [CAU_Immediate] = 1";
            //}
            //else
            //{
            //    condition = $@"WHERE ([CAU_In] = 1)
            //                   OR ([CAU_Out] = 1 OR [CAU_Pick] = 1)
            //                   OR ([CAU_Inventory] = 1)";
            //}



            //var causals = BaseBindableDbEntity.GetList<MisCfgCausal>(Global.Instance.ConnGlobal, condition);

            List<MisCfgCausal> causals = new List<MisCfgCausal>();
            foreach (var bay in HndBayBays)
            {
                causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
                {
                    CAU_Code = bay.BAY_BYT_Code,
                    CAU_Desc = bay.CfgType.BYT_Desc
                });

            }
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "OUT",
            //    CAU_Desc = "Shipment"
            //});
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "PK",
            //    CAU_Desc = "Picking"
            //});




            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "TRASLOIN",
            //    CAU_Desc = "Traslo"
            //});
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "UOVAIN",
            //    CAU_Desc = "Uova"
            //});
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "PALLETEMPTY",
            //    CAU_Desc = "Pallet Vuoti"
            //});
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "DECANTING",
            //    CAU_Desc = "Decanting"
            //});
            //causals.Add(new MisCfgCausal((SqlConnection)Global.Instance.ConnGlobal)
            //{
            //    CAU_Code = "ROBOT",
            //    CAU_Desc = "Robot"
            //});

            if (causals != null)
                Causals = causals.OrderBy(c => c.CAU_ReEntry ? 0 : 1).Select(c => new CustomComboBoxItem(c.CAU_Desc, c.CAU_Code)).ToList();
        }

        private void LoadBays()
        {
            string condition = null;

            if (_operation.HasValue && !string.IsNullOrEmpty(_causal))
            {

                //condition = $@"INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
                //               WHERE [BAY_Immediate] = 1";

                condition = $@"INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
                               WHERE [BAY_Immediate] = 1
                               AND [BYT_Code] LIKE '%{_causal}%'";

                //condition = $@"INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
                //               WHERE [BAY_Immediate] = 1
                //               AND [BAY_Desc] LIKE '%{_causal}%'";

                //condition = $@" INNER JOIN [HND_BAY_CFG_TYPES] ON [BYT_Code] = [BAY_BYT_Code]
                //                INNER JOIN [HND_BAY_MODULES]
                //             ON [BAY_Num] = [BMO_BAY_Num]
                //                INNER JOIN [HND_MODULES]
                //             ON [MOD_Code] = [BMO_MOD_Code]
                //                WHERE 
                //                (([BYT_In] = 1 AND {_operation.Value.ToString().SqlFormat()} = 'Drop')
                //                OR ([BYT_Out] = 1 AND {_operation.Value.ToString().SqlFormat()} <> 'Drop'))
                //                AND [BAY_Immediate] = 1
                //                AND [MOD_ZONE] = {_causal.SqlFormat()}";
            }

            var allBays = BaseBindableDbEntity.GetList<HndBayBay>(Global.Instance.ConnGlobal, condition);
            var bays = new List<HndBayBay>();
            foreach(HndBayBay bay in allBays)
            {
                if (bays.Any(b => b.BAY_Num == bay.BAY_Num)) continue;
                bays.Add(bay);
            }
            if (bays != null)
            {
                Bays = bays.OrderBy(b => b.BAY_Order).Select(b => new CustomComboBoxItem(b.BAY_Desc, b.BAY_Num)).ToList();
            }
            if (BAY_Num.HasValue && !bays.Any(b => b.BAY_Num == BAY_Num.Value))
            {
                BAY_Num = null;
            }
            else if (bays.Count == 1)
            {
                BAY_Num = bays.First().BAY_Num;
            }
        }

        private void SetItem(string ItemCode)
        {
            if (string.IsNullOrWhiteSpace(ItemCode))
            {
                return;
            }

            var item = Items.FirstOrDefault(i => i.ITM_Code.TrimUI() == ItemCode.TrimUI());
            if (item == null)
            {
                return;
            }

            ITM_Code = item.ITM_Code;
            ITM_Desc = item.ITM_Desc;
        }

        private void RefreshOperationIsValid()
        {
            OperationIsValid = (!string.IsNullOrWhiteSpace(UDC_Code) && UDC_Code.Length >= UdcLength ||
                                !string.IsNullOrWhiteSpace(ITM_Code)) &&
                               BAY_Num.HasValue && BAY_Num.Value > 0 &&
                               !string.IsNullOrWhiteSpace(Causal) &&
                               RequestedQuantity > 0;
        }

        private void LoadUdcFromDb(string udcCode)
        {
            if (!string.IsNullOrEmpty(udcCode))
            {
                var _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
                var udc = new AgilogDll.UdcUdc(_conn);
                if (udc.GetByKey(udcCode, _conn, false))
                {
                    Udc = udc;
                }
                else
                {
                    Udc = null;
                }
            }
            else
            {
                Udc = null;
            }
        }

        #endregion

        #region Events

        private void _init_OperationCompleted(object sender, GenericEventArgs e)
        {
            LoadingIsVisible = false;

            if (_operation.HasValue &&
                Causals.Count > 0) Causal = Causals.First().Value.ToString();

            SetItem(ITM_Code);

            NotifyOfPropertyChange(() => Dictionary_ItemCodes);
            NotifyOfPropertyChange(() => Dictionary_ItemDescriptions);
            NotifyOfPropertyChange(() => Dictionary_Udcs);
            NotifyOfPropertyChange(() => Bays);
            NotifyOfPropertyChange(() => Causals);
            NotifyOfPropertyChange(() => UDC_Code);
            NotifyOfPropertyChange(() => BAY_Num);
            NotifyOfPropertyChange(() => Causal);
            NotifyOfPropertyChange(() => SelectBay);
        }

        public void ItemCodeAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null) return;

            SetItem(element.Value.ToString());
        }

        public void ItemDescriptionAutoCompleted(RoutedEventArgs e)
        {
            var element = ((GenericRoutedEventArgs)e).Argument as CustomDictionaryItem;
            if (element == null) return;

            SetItem(element.Value.ToString());
        }

        public void QuantityUp()
        {
            if (RequestedQuantity >= decimal.MaxValue) RequestedQuantity = 0;
            else RequestedQuantity++;
        }

        public void QuantityDown()
        {
            if (RequestedQuantity <= 0) return;

            RequestedQuantity--;
        }

        public async void Accept()
        {
            var message = Global.Instance.LangTl("Do you confirm the operation?");
            if (!await Global.ConfirmAsync(_windowManager, message)) return;

            LoadingIsVisible = true;

            HndBayBay bay = HndBayBays.FirstOrDefault(b => b.BAY_Num == BAY_Num.Value);

            var error = await _utils.ImmediateAsync(
                UDC_Code,
                ITM_Code,
                Owner,
                BAY_Num.Value,
                //Causal,
                //"USC",
                bay != null ? bay.BAY_BYT_Code == "PK" ? "PK" : "USC" : "USC",
                Notes,
                RequestedQuantity);

            LoadingIsVisible = false;

            if (!string.IsNullOrWhiteSpace(error)) Global.ErrorAsync(_windowManager, error);
            else
            {
                await Global.InfoAsync(_windowManager, Global.Instance.LangTl("Operation launched successfully!"));
                await TryCloseAsync();
            }
        }

        public void Refuse()
        {
            TryCloseAsync();
        }

        #endregion

    }
}
