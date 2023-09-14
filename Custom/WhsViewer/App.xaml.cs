using mSwDllWPFUtils;
using System;
using System.Windows;

namespace WhsViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1012);

            if (!Global.Instance.Initialize(true))
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
