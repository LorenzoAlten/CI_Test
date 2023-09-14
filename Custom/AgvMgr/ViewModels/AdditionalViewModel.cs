using Caliburn.Micro;
using mSwDllUtils;
using mSwDllWPFUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using mSwAgilogDll.SEW;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace AgvMgr.ViewModels
{
    [Export(typeof(AdditionalViewModel))]
    public class AdditionalViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Properties

        public SEW_AGV Agv { get; set; }

        public ObservableCollection<SEWAdditionalData> AdditionalList { get; set; } = new ObservableCollection<SEWAdditionalData>();

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AdditionalViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SEW_AGV agv)
        {
            Agv = agv;
            DisplayName = Agv.AGV_Code;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region ViewModel Override

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            RefreshAll();
            Global.Instance.OnEvery1Sec += Instance_OnEvery1Sec;

            return base.OnActivateAsync(cancellationToken);
        }

        #endregion

        #region Public methods
        public void ReloadTrackData()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel();

            cmd_telegram.Service = SEW_Agv_Service.NO_SERVICE;
            cmd_telegram.Part_data = 0;
            cmd_telegram.UDP_receive_port = 0;
            cmd_telegram.Index = 0;
            cmd_telegram.Program_number = 0;
            cmd_telegram.Commands = SEW_Commands.Reload_track_data;

            Global.Instance.SendTelegram((int)Agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
        }
        private void Instance_OnEvery1Sec(object sender, GenericEventArgs e)
        {
            RefreshAll();
        }

        public void RefreshAll()
        {
            string desc;

            AdditionalList.Clear();

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.Diagnostic_Sick_1)(BitConverter.ToUInt16(Agv.ErreviAdditional, 0))));
            AdditionalList.Add(new SEWAdditionalData("Sick diagnostic 1", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.Diagnostic_Sick_2)(BitConverter.ToUInt16(Agv.ErreviAdditional, 2))));
            AdditionalList.Add(new SEWAdditionalData("Sick diagnostic 2", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.Diagnostic_Sick_3)(BitConverter.ToUInt16(Agv.ErreviAdditional, 4))));
            AdditionalList.Add(new SEWAdditionalData("Sick diagnostic 3", desc));

            string nameVtrackError = Enum.GetName(((SEWAdditionalData.Vtrack_Error)Agv.ErreviAdditional[5]).GetType(), ((SEWAdditionalData.Vtrack_Error)Agv.ErreviAdditional[5]));
            if (nameVtrackError != null)
            {
                FieldInfo field = ((SEWAdditionalData.Vtrack_Error)Agv.ErreviAdditional[5]).GetType().GetField(nameVtrackError);
                DescriptionAttribute attr =
                               Attribute.GetCustomAttribute(field,
                                 typeof(DescriptionAttribute)) as DescriptionAttribute;
                desc = attr.Description;
                AdditionalList.Add(new SEWAdditionalData("Vtrack Detected", desc));
            }

            desc = (BitConverter.ToUInt32(Agv.ErreviAdditional, 6)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum Flexi", desc));

            desc = (BitConverter.ToUInt32(Agv.ErreviAdditional, 10)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum Scanner Front", desc));

            desc = (BitConverter.ToUInt32(Agv.ErreviAdditional, 14)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum Scanner Rear", desc));

            desc = SEWAdditionalData.GetDescription((SEWAdditionalData.LHD_State_1)Agv.ErreviAdditional[20]);
            AdditionalList.Add(new SEWAdditionalData("LHD 1", desc));

            desc = Agv.ErreviAdditional[22].ToString();
            AdditionalList.Add(new SEWAdditionalData("Conveyor Interface", desc));

            desc = SEWAdditionalData.GetDescription((SEWAdditionalData.LHD_State_2)Agv.ErreviAdditional[24]);
            AdditionalList.Add(new SEWAdditionalData("LHD 2", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.LHD_ErrorList_1)(BitConverter.ToUInt16(Agv.ErreviAdditional, 26))));
            AdditionalList.Add(new SEWAdditionalData("LHD Error List 1", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.LHD_ErrorList_2)(BitConverter.ToUInt16(Agv.ErreviAdditional, 28))));
            AdditionalList.Add(new SEWAdditionalData("LHD Error List 2", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.LHD_WarningList_1)(BitConverter.ToUInt16(Agv.ErreviAdditional, 30))));
            AdditionalList.Add(new SEWAdditionalData("LHD Warning List 1", desc));

            desc = SEWAdditionalData.GetDescription(((SEWAdditionalData.LHD_WarningList_2)(BitConverter.ToUInt16(Agv.ErreviAdditional, 32))));
            AdditionalList.Add(new SEWAdditionalData("LHD Warning List 2", desc));

            desc = (BitConverter.ToUInt64(Agv.ErreviAdditional, 34)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum Standard", desc));

            desc = (BitConverter.ToUInt64(Agv.ErreviAdditional, 42)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum Safety", desc));

            desc = (BitConverter.ToUInt64(Agv.ErreviAdditional, 50)).ToString("X");
            AdditionalList.Add(new SEWAdditionalData("CheckSum TextList", desc));
        }
        #endregion

    }
    public class SEWAdditionalData : BaseBindableDbEntity
    {
        private string _data;
        private string _value;
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        public SEWAdditionalData(string data, string value)
        {
            Data = data;
            Value = value;
        }

        public static string GetDescription(Enum value)
        {
            Type type = value.GetType();
            var values = Enum.GetValues(type);
            var setValues = new List<Enum>();
            foreach (var enumValue in values)
            {
                if (value.HasFlag((Enum)enumValue))
                    setValues.Add((Enum)enumValue);
            }
            var stringList = new List<string>();
            foreach (var singleValue in setValues)
            {
                var name = Enum.GetName(type, singleValue);
                if (name != null)
                {
                    FieldInfo field = type.GetField(name);
                    if (field != null)
                    {
                        DescriptionAttribute attr =
                               Attribute.GetCustomAttribute(field,
                                 typeof(DescriptionAttribute)) as DescriptionAttribute;
                        //-----------------------------------------------
                        // Se ho definito l'attributo restituisco quello
                        //-----------------------------------------------
                        if (attr != null)
                        {
                            stringList.Add(attr.Description);
                        }
                        //---------------------------------------------------------------
                        // Se non ho definito l'attributo restituisco il nome dell'enum
                        //---------------------------------------------------------------
                        else
                        {
                            stringList.Add(name);
                        }
                    }
                }
            }
            return string.Join(", ", stringList.ToArray());
        }
        [Flags]
        public enum Diagnostic_Sick_1 : ushort
        {
            [Description("Errore Comunicazione Sick")]
            ErrComm = 0x01,                 // Bit 0
            [Description("Fungo premuto")]
            EStop = 0x02,                   // Bit 1
            [Description("Scanner Fr - Campo protetto")]
            Scn01_PF = 0x04,                // Bit 2
            [Description("Scanner Fr - Campo allerta 01")]
            Scn01_WF1 = 0x08,               // Bit 3
            [Description("Scanner Fr - Campo allerta 02")]
            Scn01_WF2 = 0x10,               // Bit 4
            [Description("Scanner Fr - Errore")]
            Scn01_Err = 0x20,               // Bit 5
            [Description("Scanner Fr - Ottica sporca")]
            Scn01_Contamination = 0x40,     // Bit 6
            [Description("Scanner Re - Campo protetto")]
            Scn02_PF = 0x80,                // Bit 7
            [Description("Scanner Re - Campo allerta 01")]
            Scn02_WF1 = 0x100,              // Bit 8
            [Description("Scanner Re - Campo allerta 02")]
            Scn02_WF2 = 0x200,              // Bit 9
            [Description("Scanner Re - Errore")]
            Scn02_Err = 0x400,              // Bit 10
            [Description("Scanner Re - Ottica sporca")]
            Scn02_Contamination = 0x800,    // Bit 11
            [Description("SLS bypass intervenuto")]
            SLS_Bypass = 0x1000,            // Bit 12
            [Description("SLS TOS (Turn On Spot) intervenuto")]
            SLS_TOS = 0x2000,               // Bit 13
            [Description("SLS manuale intervenuto")]
            SLS_Man = 0x4000,               // Bit 14
            [Description("SLS automatico con carico a bordo intervenuto")]
            SLS_AutoPieno = 0x8000,         // Bit 15
        }
        public enum Diagnostic_Sick_2 : ushort
        {
            [Description("SLS automatico senza carico a bordo intervenuto")]
            SLS_AutoVuoto = 0x01,           // Bit 0
            [Description("Controllo media velocià intervenuto")]
            SpeedCrossCheck = 0x02,         // Bit 1
            [Description("Motore Sx errore velocità")]
            MotLe_Speed = 0x04,             // Bit 2
            [Description("Motore Dx errore velocità")]
            MotRi_Speed = 0x08,             // Bit 3
            [Description("Motore Sx STO (Safe Torque Off)")]
            MotLe_STO = 0x10,               // Bit 4
            [Description("Motore Dx STO (Safe Torque Off)")]
            MotRi_STO = 0x20,               // Bit 5
            [Description("Motore Sx errore controllo rampa SS1")]
            MotLe_SS1Err = 0x40,            // Bit 6
            [Description("Motore Dx errore controllo rampa SS1")]
            MotRi_SS1Err = 0x80,            // Bit 7 
            [Description("STO Ruote")]
            STO_Ruote = 0x100,              // Bit 8
            [Description("STO Rulli/Top module")]
            STO_Rulli = 0x200,              // Bit 9
            [Description("STO fermi meccanici")]
            STO_Fermi = 0x400,              // Bit 10
            [Description("SS0")]
            SS0 = 0x800,                    // Bit 11
            [Description("SS1 da Scanner")]
            SS1_Scanner = 0x1000,           // Bit 12 
            [Description("SS1 da Fermi")]
            SS1_Fermi = 0x2000,             // Bit 13
            [Description("SS2")]
            SS2 = 0x4000,                   // Bit 14
            [Description("Errore EDM (external device monitoring) Ruote")]
            EDM_RuoteErr = 0x8000,          // Bit 15
        }
        public enum Diagnostic_Sick_3 : byte
        {
            [Description("Errore EDM (external device monitoring) Rulli/Top module")]
            EDM_RulliErr = 0x01,    // Bit 0
            [Description("Errore EDM (external device monitoring) Fermi")]
            EDM_FermiErr = 0x02,    // Bit 1
            Bit_2 = 0x04,           // Bit 2
            Bit_3 = 0x08,           // Bit 3
            Bit_4 = 0x10,           // Bit 4
            Bit_5 = 0x20,           // Bit 5
            Bit_6 = 0x40,           // Bit 6
            [Description("Vtrack riconosciuta")]
            Vtrack_Detected = 0x80, // Bit 7
        }
        public enum Vtrack_Error : byte
        {
            [Description("Vtrack NO Error")]
            Vtrack_NO_Error = 0,    
            [Description("VTrack agv not on start transponder")]
            VTrack_agv_not_on_start_transponder = 1,
            [Description("VTRACK ID NOT FOUND")]
            VTRACK_ID_NOT_FOUND = 2,
            [Description("VTRACK LARGE DISTANCE TO START OF SEGMENT")]
            VTRACK_LARGE_DISTANCE_TO_START_OF_SEGMENT = 3,
            [Description("VTRACK END OF TRACK REACHED")]
            VTRACK_END_OF_TRACK_REACHED = 4,
            [Description("VTRACK NO SEGMENTS")]
            VTRACK_NO_SEGMENTS = 5,
            [Description("VTRACK END TRANSPONDER PASSED")]
            VTRACK_END_TRANSPONDER_PASSED = 6,
            [Description("VTRACK INITIALIZATION DISTANCE TOO BIG")]
            VTRACK_INITIALIZATION_DISTANCE_TOO_BIG = 7,
            [Description("VTRACK LARGE DISTANCE TO TRACK POSITION")]
            VTRACK_LARGE_DISTANCE_TO_TRACK_POSITION = 8,
            [Description("VTRACK INITIALIZATION NOT POSSIBLE")]
            VTRACK_INITIALIZATION_NOT_POSSIBLE = 9,
        }
        public enum LHD_State_1 : ushort
        {
            Ready = 0x01,                       // Bit 0
            Error = 0x02,                       // Bit 1
            Occupied = 0x04,                    // Bit 2
            [Description("Infeed From Station Finished")]
            InfeedFromStationFinished = 0x08,   // Bit 3
            [Description("Outfeed To Station Finished")]
            OutfeedToStationFinished = 0x10,    // Bit 4
            [Description("Lift In Lower Position")]
            LiftInLowerPosition = 0x20,         // Bit 5
            [Description("Lift In Upper Position")]
            LiftInUpperPosition = 0x40,         // Bit 6
            Bit_7 = 0x80,           // Bit 7
            Bit_8 = 0x100,          // Bit 8
            Bit_9 = 0x200,          // Bit 9
            Bit_10 = 0x400,         // Bit 10
            Bit_11 = 0x800,         // Bit 11
            Bit_12 = 0x1000,        // Bit 12
            Bit_13 = 0x2000,        // Bit 13
            Bit_14 = 0x4000,        // Bit 14
            Bit_15 = 0x8000,        // Bit 15
        }
        public enum LHD_State_2 : ushort
        {
            [Description("TO Ready")]
            TO_Ready = 0x01,        // Bit 0
            [Description("TO Active")]
            TO_Active = 0x02,       // Bit 1
            [Description("TO End")]
            TO_End = 0x04,          // Bit 2
            [Description("HO Ready")]
            HO_Ready = 0x08,        // Bit 3
            [Description("HO Active")]
            HO_Active = 0x10,       // Bit 4
            [Description("HO End")]
            HO_End = 0x20,          // Bit 5
            GapCheckRi = 0x40,      // Bit 6
            GapCheckLe = 0x80,      // Bit 7
            PkEnd = 0x100,          // Bit 8
            DpEnd = 0x200,          // Bit 9
            Bit_10 = 0x400,         // Bit 10 
            Bit_11 = 0x800,         // Bit 11
            Bit_12 = 0x1000,        // Bit 12
            Bit_13 = 0x2000,        // Bit 13
            [Description("Abilitato controllo fotocellula posizionamento")]
            ControlloFtcPos = 0x4000,        // Bit 14 - Mi indica che posso controllare lo stato della fotocellula di posizionamento 
                                             // e non devo attendere che il valore di FtcPosOk sia calcolato correttamente
            [Description("AGV posizionato correttamente")]
            FtcPosOk = 0x8000,        // Bit 15
        }
        public enum LHD_ErrorList_1 : ushort
        {
            [Description("ERRORE NODO PROFINET NR 05 MOVIPLC")]
            Error_1 = 0x01,                       // Bit 0
            [Description("ERRORE NODO PROFINET NR 11 CENTRALINA SICK")]
            Error_2 = 0x02,                       // Bit 1
            [Description("MANCA AUXON CATENARIA")]
            Error_3 = 0x04,                    // Bit 2
            Error_4 = 0x08,     // Bit 3
            Error_5 = 0x10,     // Bit 4
            Error_6 = 0x20,     // Bit 5
            Error_7 = 0x40,     // Bit 6
            Error_8 = 0x80,     // Bit 7
            Error_9 = 0x100,    // Bit 8
            Error_10 = 0x200,   // Bit 9
            [Description("ERRORE NODO PROFINET NR 35 INVERTER TB4200")]
            Error_11 = 0x400,         // Bit 10
            [Description("ALLARME INVERTER TB4200 CATENARIA")]
            Error_12 = 0x800,         // Bit 11
            [Description("INVERTER TB4200 CATENARIA NON IN RUN")]
            Error_13 = 0x1000,        // Bit 12
            [Description("TIMEOUT APERTURA UNITA' DI PRESA")]
            Error_14 = 0x2000,        // Bit 13
            [Description("TIMEOUT CHIUSURA UNITA' DI PRESA")]
            Error_15 = 0x4000,        // Bit 14
            [Description("INTASAMENTO O POSIZIONAMENTO ERRATO FTC BG4300 (GAP IN)")]
            Error_16 = 0x8000,        // Bit 15
        }
        public enum LHD_ErrorList_2 : ushort
        {
            [Description("INTASAMENTO O POSIZIONAMENTO ERRATO FTC BG4301 (PRES)")]
            Error_17 = 0x01,                       // Bit 0
            [Description("INTASAMENTO O POSIZIONAMENTO ERRATO FTC BG4302 (GAP OUT)")]
            Error_18 = 0x02,                       // Bit 1
            [Description("FOTOCELLULA DI SICUREZZA BG4300 INTERCETTATA (GAP IN)")]
            Error_19 = 0x04,                    // Bit 2
            [Description("FOTOCELLULA DI SICUREZZA BG4302 INTERCETTATA (GAP OUT)")]
            Error_20 = 0x08,   // Bit 3
            [Description("INCONGRUENZA BARRIERA SICUREZZA E FTC CATENARIA")]
            Error_21 = 0x10,    // Bit 4
            [Description("TIME OUT TRASFERIMENTO DA TERRA A AGV")]
            Error_22 = 0x20,         // Bit 5
            [Description("TIME OUT TRASFERIMENTO DA AGV A TERRA")]
            Error_23 = 0x40,         // Bit 6
            [Description("PRESENZA FISICA SENZA PRESENZA LOGICA")]
            Error_24 = 0x80,           // Bit 7
            [Description("PRESENZA LOGICA SENZA PRESENZA FISICA")]
            Error_25 = 0x100,          // Bit 8
            Error_26 = 0x200,          // Bit 9
            Error_27 = 0x400,         // Bit 10
            Error_28 = 0x800,         // Bit 11
            Error_29 = 0x1000,        // Bit 12
            Error_30 = 0x2000,        // Bit 13
            Error_31 = 0x4000,        // Bit 14
            Error_32 = 0x8000,        // Bit 15
        }
        public enum LHD_WarningList_1 : ushort
        {
            Warning_1 = 0x01,       // Bit 0
            Warning_2 = 0x02,       // Bit 1
            Warning_3 = 0x04,       // Bit 2
            Warning_4 = 0x08,       // Bit 3
            Warning_5 = 0x10,       // Bit 4
            Warning_6 = 0x20,       // Bit 5
            Warning_7 = 0x40,       // Bit 6
            Warning_8 = 0x80,       // Bit 7
            Warning_9 = 0x100,      // Bit 8
            Warning_10 = 0x200,     // Bit 9
            Warning_11 = 0x400,     // Bit 10
            Warning_12 = 0x800,     // Bit 11
            Warning_13 = 0x1000,    // Bit 12
            Warning_14 = 0x2000,    // Bit 13
            Warning_15 = 0x4000,    // Bit 14
            Warning_16 = 0x8000,    // Bit 15
        }
        public enum LHD_WarningList_2 : ushort
        {
            [Description("TIMEOUT COMUNICAZIONE CON MOVIPLC")]
            Warning_17 = 0x01,       // Bit 0
            [Description("TIMEOUT COMUNICAZIONE CON CENTRALINA SICK")]
            Warning_18 = 0x02,       // Bit 1
            [Description("TIMEOUT COMUNICAZIONE CON PLC TERRA")]
            Warning_19 = 0x04,       // Bit 2
            [Description("TIMEOUT COMUNICAZIONE CON PC")]
            Warning_20 = 0x08,       // Bit 3
            Warning_21 = 0x10,       // Bit 4
            Warning_22 = 0x20,       // Bit 5
            Warning_23 = 0x40,       // Bit 6
            Warning_24 = 0x80,       // Bit 7
            Warning_25 = 0x100,      // Bit 8
            Warning_26 = 0x200,     // Bit 9
            Warning_27 = 0x400,     // Bit 10
            Warning_28 = 0x800,     // Bit 11
            Warning_29 = 0x1000,    // Bit 12
            Warning_30 = 0x2000,    // Bit 13
            Warning_31 = 0x4000,    // Bit 14
            Warning_32 = 0x8000,    // Bit 15
        }
    }
}
