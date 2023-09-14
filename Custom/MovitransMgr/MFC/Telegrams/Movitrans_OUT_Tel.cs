using mSwAgilogDll.SEW;
using mSwDllMFC;
using mSwDllUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Movitrans
{
    public class Movitrans_OUT_Tel : Movitrans_Tel
    {
        #region Members

        #endregion

        #region Properties
        public class MOV_UDPData
        {
            public MOV_Status_Word Status_Word { get; set; }
            public short ErrorCode { get; set; }
            public short WarningCode { get; set; }
            public short Temperature { get; set; }
            public short Utilization { get; set; }
            public short OutputPower { get; set; }
        }
        public List<MOV_UDPData> ReceivedData = new List<MOV_UDPData>(8);
        #endregion

        #region Constructor/Destructor

        public Movitrans_OUT_Tel() : base()
        {
            TelegramType = SEW_Agv_TelTypes.OUT;
        }

        #endregion

        #region Public Methods
        public override void ParseReceivedMessage(string message, out Telegram responseTelegram)
        {
            responseTelegram = null;

            //-----------------------------------------------------------------------
            // Tutti i messaggi sono ControllerStatusMessage?? BO però così
            // almeno ogni lettura mi fa il Refresh delle variabili pubbliche
            // vedremo se va bene così
            //-----------------------------------------------------------------------
            ControllerStatusMessage = true;

            //----------------------------------------------------------------------------------------------------
            // I controller gestiscono i messaggi come stringhe ma il protocollo SEW 
            // codifica un array di WORD quindi converto
            //----------------------------------------------------------------------------------------------------
            Encoding encoding = Encoding.GetEncoding("iso-8859-1"); //(28591);  //Europa occidentale (ISO)
            List<byte> byt_mess = encoding.GetBytes(message).ToList();
            for(int i = 0; i < 8; i++)
            {
                ReceivedData.Add(new MOV_UDPData());
                ReceivedData.LastOrDefault().Status_Word = (MOV_Status_Word)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i+1)));
                ReceivedData.LastOrDefault().ErrorCode = (short)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i + 1) + 4));
                ReceivedData.LastOrDefault().WarningCode = (short)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i + 1) + 6));
                ReceivedData.LastOrDefault().Temperature = (short)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i + 1) + 8));
                ReceivedData.LastOrDefault().Utilization = (short)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i + 1) + 10));
                ReceivedData.LastOrDefault().OutputPower = (short)BitConverter.ToInt16(byt_mess.ToArray(), (40 * (i + 1) + 12));
            }
        }
        #endregion
    }
    [Flags]
    public enum MOV_Status_Word : ushort
    {
        Movitrans_Enabled = 0x01,               // Bit 0
        TPS_Voltage_Limit = 0x02,               // Bit 1
        Error = 0x04,                           // Bit 2
        Warning = 0x08,                         // Bit 3
        Remote_device_Connected = 0x10,         // Bit 4
        Switch_Cabinet_Enabled = 0x20,          // Bit 5
        TES_Ready = 0x40,                       // Bit 6
        TES_PO_Data_Enabled = 0x80,             // Bit 7
        TES_End_of_Ramp = 0x100,                // Bit 8
        Initialization_Error = 0x200,           // Bit 9
        TPS_Connected = 0x400,                  // Bit 10
        TES_Connected = 0x800,                  // Bit 11
        Read_Param_Done = 0x1000,               // Bit 12
        Read_Param_Error = 0x2000,              // Bit 13
        Write_Param_Done = 0x4000,              // Bit 14
        Write_Param_Error = 0x8000,             // Bit 15
    }

    public enum MOV_Warning : ushort
    {
        Nessuna_Abilitazione = 2,               
        Abilitazione = 4,               
        NonPronto = 21,                           
    }

    public enum MOV_Error : ushort
    {
        Sovracorrente = 1,
        AnomaliaControlloUCE = 2,
        MancanzaFaseRete = 6,
        TensioneCircuitoIntermedio = 7,
        Sovratemperatura = 11,
        MorsettoEsterno = 26,
        TimeoutSBus2 = 46,
        TimeoutSBus1 = 47,
        Hardware = 50,
        SincronizzazioneEsterna = 68,
        SezionePotenza = 196,
    }
}
