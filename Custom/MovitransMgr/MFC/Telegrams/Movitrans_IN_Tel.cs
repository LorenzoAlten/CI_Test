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
    public class Movitrans_IN_Tel : Movitrans_Tel
    {
        #region Members

        public class MOV_UDPData
        {
            #region Members/Properties

            public MOV_Control_Word Control_Word { get; set; }
            public short SetPoint { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        public List<MOV_UDPData> SendData
        {
            get { return GetProperty<List<MOV_UDPData>>(); }
            set { SetProperty(value); }
        }
        public short Receive_Port
        {
            get { return GetProperty<short>(); }
            set { SetProperty(value); }
        }
        
        #endregion

        #region Constructor/Destructor

        public Movitrans_IN_Tel() : base()
        {
            TelegramType = SEW_Agv_TelTypes.IN;

            SendData = new List<MOV_UDPData>(8);
        }

        #endregion

        #region Public Methods
       
        public override string GetMessage()
        {
            string message;
            //----------------------------------------------------
            // Compongo L'array di WORD da inviare
            //----------------------------------------------------
            List<byte> byt_mess = new List<byte>();
            byt_mess.Add(0);
            byt_mess.Add(0);
            byt_mess.AddRange(BitConverter.GetBytes(Receive_Port).ToList());
            for (; byt_mess.Count < 40;)
            {
                byt_mess.Add(0);
            }
            foreach(MOV_UDPData mov in SendData){
                byt_mess.AddRange(BitConverter.GetBytes((short)mov.Control_Word).ToList());
                byt_mess.AddRange(BitConverter.GetBytes((short)mov.SetPoint).ToList());
                for(int i=0;i<36;i++)
                {
                    byt_mess.Add(0);
                }
            }
            //----------------------------------------------------------------------------------------------------
            // Trasformo l'array in una stringa dato che poi i controller gestiscono i messaggi come stringhe
            //----------------------------------------------------------------------------------------------------
            Encoding encoding = Encoding.GetEncoding("iso-8859-1"); //(28591);  //Europa occidentale (ISO)
            message = encoding.GetString(byt_mess.ToArray());

            return message;
        }

        #endregion
    }
    [Flags]
    public enum MOV_Control_Word : short
    {
        Movitrans_Enable = 0x01,            // Bit 0
        Movitrans_Reset = 0x02,             // Bit 1
        Read_Param_Execute = 0x04,          // Bit 2
        Write_Param_Execute = 0x08,         // Bit 3
    }
}
