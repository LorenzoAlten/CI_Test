using mSwDllWPFUtils;
using System;
using System.Windows;

namespace PackDataViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int PLN_Id { get; private set; }
        public static int PRC_Num { get; private set; }
        public static int TRK_Index { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(65);

            if (!Global.Instance.Initialize(true, false, false, false, false, true))
            {
                Environment.Exit(0);
            }

            Global.Instance.ApplyTheme();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Global.Instance.App_Closing();

            base.OnExit(e);

            Global.Instance.App_Closed();
        }
    }
}
