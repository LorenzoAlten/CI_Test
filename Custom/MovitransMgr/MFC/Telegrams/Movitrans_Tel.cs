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
    public class Movitrans_Tel : Telegram
    {
        #region Members

        protected string _separator = "";

        #endregion

        #region Properties

        public SEW_Agv_TelTypes TelegramType { get; set; }

        #endregion

        #region Constructor/Destructor

        public Movitrans_Tel() : base()
        {
            TelegramType = SEW_Agv_TelTypes.UNKNOWN;
            FixedLength = 140;
        }

        #endregion

        #region Public Methods
       
        #endregion
    }

}
