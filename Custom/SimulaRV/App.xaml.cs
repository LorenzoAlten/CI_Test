using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace SimulaRV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1102);

            // Non istanzio le comunicazioni
            if (!Global.Instance.Initialize(true, true, false, false, false, true))
            {
                Environment.Exit(0);
            }

            Global.Instance.ApplyTheme();

            ParamManager.Init(Global.Instance.ConnGlobal);
            Logger.Init(Global.Instance.DVC.Code, Global.Instance.ConnGlobal);

            base.OnStartup(e);
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            Global.Instance.App_Activated();

            base.OnLoadCompleted(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Global.Instance.App_Closing();

            base.OnExit(e);

            Global.Instance.App_Closed();
        }
    }
}
