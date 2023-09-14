using mSwAgilogDll;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace OrdersMgr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var global = new Global(1014);

            if (!Global.Instance.Initialize(true))
            {
                Environment.Exit(0);
            }

            Global.Instance.ApplyTheme();

            MasterDataManager.Conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);

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
