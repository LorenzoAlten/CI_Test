using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace AstarMgr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1103);

            if (!Global.Instance.Initialize(true))
            {
                Environment.Exit(0);
            }

            Global.Instance.ApplyTheme();

            Global.Instance.Log("AstarMgr avviato", LogLevels.System);

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
