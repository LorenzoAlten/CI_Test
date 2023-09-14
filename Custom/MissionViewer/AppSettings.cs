namespace MissionViewer
{
    public class AppSettings : mSwDllUtils.AppSettings
    {
        public static AppSettings Instance
        {
            get { return GetInstance() as AppSettings; }
        }

        public bool ShowExceptionFields { get; set; }
    }
}
