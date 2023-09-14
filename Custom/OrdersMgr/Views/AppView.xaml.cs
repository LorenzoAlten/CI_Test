using System;
using System.Windows;

namespace OrdersMgr.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        public AppView()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //Global.Instance.App_Activated();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Topmost = true;
            Activate();
            Topmost = false;
        }
    }
}
