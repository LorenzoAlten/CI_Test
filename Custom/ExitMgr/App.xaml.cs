using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExitMgr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1016);

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
