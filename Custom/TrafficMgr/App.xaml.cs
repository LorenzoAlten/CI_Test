using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace TrafficMgr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1101);

            // Non istanzio le comunicazioni perché il TrafficMgr
            // è autonomo, stile CommsMgr
            if (!Global.Instance.Initialize(true, false))
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
