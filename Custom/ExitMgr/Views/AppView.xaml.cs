using ExitMgr.ViewModels;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExitMgr.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        public AppView()
        {
            InitializeComponent();

            DataContextChanged += AppView_DataContextChanged;
        }

        private void AppView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var data = DataContext as AppViewModel;
            if (data == null) return;

            data.OnTelegramReceived += Data_OnTelegramReceived;
        }

        private void Data_OnTelegramReceived(object sender, mSwDllUtils.GenericEventArgs e)
        {
            if (this.IsActive) return;

            Topmost = true;
            Activate();
            Focus();
            Topmost = false;
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
