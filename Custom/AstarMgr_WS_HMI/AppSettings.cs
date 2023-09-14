namespace AstarMgr_WS_HMI
{
    public class AppSettings : mSwDllUtils.AppSettings
    {
        public static AppSettings Instance
        {
            get { return GetInstance() as AppSettings; }
        }

        public string XmlRelativePath { get; set; }
        public int CHL_Id { get; set; }
    }
}
