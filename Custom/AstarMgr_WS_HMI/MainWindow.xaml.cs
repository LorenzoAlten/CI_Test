using AgilogDll.MFC.Astar;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;

namespace AstarMgr_WS_HMI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        private AstarWSLauncher _astarWSLauncher;

        // Istanzio l'icona nella barra delle notifiche
        private System.Windows.Forms.NotifyIcon _NotifyIcon;
        Thread _serviceEngine;

        #endregion

        #region Load

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Mostra icona nella barra delle notifiche
            // Il tempo è impostato a 0 perchè comunque il minimo è 10 sec.
            // Conteggiati solo se accade qualcosa (movimento mouse, pressione tasto, ecc...)
            ShowNotifyIcon("Astar Windows Service", $"Astar Windows Service started{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}",
                System.Windows.Forms.ToolTipIcon.Info, 0);

            Hide();

            Start();
        }

        #endregion

        #region Close

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Stop();

            // Disabilito la visualizzazione dell'icona nella barra delle notifiche
            _NotifyIcon.Visible = false;
        }

        #endregion

        #region Events

        private void Start()
        {
            if (_astarWSLauncher != null) { return; }

            _serviceEngine = new Thread(new ThreadStart(Init));
            _serviceEngine.Start();
        }

        private void Stop()
        {
            try
            {
                _serviceEngine?.Abort();
            }
            catch { }

            if (_astarWSLauncher != null)
            {
                _astarWSLauncher.Dispose();
                _astarWSLauncher = null;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) { Hide(); }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            try
            {
                var global = new Global(0);
                global.InitializeSolo(AppSettings.Instance.XmlRelativePath);

                _astarWSLauncher = new AstarWSLauncher(
                    new SqlConnection(Global.Instance.ConnGlobal.ConnectionString),
                    AppSettings.Instance.CHL_Id);

                _astarWSLauncher.OnNotify += _astarWSLauncher_OnNotify;

                _astarWSLauncher.Init();
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message);
            }
        }

        private void _astarWSLauncher_OnNotify(object sender, GenericEventArgs e)
        {
            LogInfo(e.Argument.ToString());
        }

        private void LogInfo(string Message)
        {
            //txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += Message + "\n"));
            //txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.ScrollToEnd()));
            txtInfo.Dispatcher.BeginInvoke(new Action(() => txtInfo.Text += Message + "\n"), System.Windows.Threading.DispatcherPriority.Send);
            txtInfo.Dispatcher.BeginInvoke(new Action(() => txtInfo.ScrollToEnd()), System.Windows.Threading.DispatcherPriority.Send);
        }

        private void ShowNotifyIcon(string Title, string Text, System.Windows.Forms.ToolTipIcon ToolTipIcon, int DisplayTime)
        {
            _NotifyIcon = new System.Windows.Forms.NotifyIcon();
            System.IO.Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/AstarMgr_WS_HMI;component/mSw.ico")).Stream;
            if (iconStream != null)
            {
                _NotifyIcon.Icon = new System.Drawing.Icon(iconStream);
            }
            _NotifyIcon.Text = Title;
            _NotifyIcon.BalloonTipTitle = Title;
            _NotifyIcon.BalloonTipText = Text;
            _NotifyIcon.BalloonTipIcon = ToolTipIcon;
            _NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            _NotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            _NotifyIcon.Visible = true;
            _NotifyIcon.ShowBalloonTip(DisplayTime);
        }

        #endregion
    }
}
