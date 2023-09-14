using System;
using System.Collections.Generic;

namespace TrafficMgr
{
    public sealed class Manager
    {
        #region Singleton
        private static readonly Manager instance = new Manager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Manager()
        {
        }

        private Manager()
        {
        }

        public static Manager Instance
        {
            get { return instance; }
        }
        #endregion

        internal List<MsgEntry> MsgEntries { get; } = new List<MsgEntry>();
    }

    public class MsgEntry
    {
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
        public string Dest { get; set; }
        public string Message { get; set; }
    }
}
