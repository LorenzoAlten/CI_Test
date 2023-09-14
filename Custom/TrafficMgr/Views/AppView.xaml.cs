using mSwDllWPFUtils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrafficMgr.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        #region Members

        // Icona per la barra delle notifiche
        private System.Windows.Forms.NotifyIcon _NotifyIcon;

        #endregion

        public AppView()
        {
            InitializeComponent();

            //ShowNotifyIcon("TrafficMgr",
            //    string.Format("TrafficMgr started at {0:d} {0:T}", DateTime.Now),
            //    System.Windows.Forms.ToolTipIcon.Info, 0);
        }

        #region Private Methods

        private void ShowNotifyIcon(string Title, string Text, System.Windows.Forms.ToolTipIcon ToolTipIcon, int DisplayTime)
        {
            _NotifyIcon = new System.Windows.Forms.NotifyIcon();
            System.IO.Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/TrafficMgr;component/Resources/Logo.ico")).Stream;
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

        #region Events

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Show();
            WindowState = WindowState.Maximized;
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Hide();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            //if (WindowState == WindowState.Minimized)
            //    Hide();
        }

        #endregion
    }
}
