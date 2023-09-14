using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AgvMgr.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        public static AppView Instance { get; set; }

        public AppView()
        {
            InitializeComponent();

            Instance = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.AppViewModel;
            
        }

        private async void Vm_OnSnackMessageRequested(object sender, mSwDllUtils.GenericEventArgs e)
        {
            //var messageQueue = MainSnackbar.MessageQueue;

            //await Task.Factory.StartNew(() => messageQueue.Enqueue(e.Argument.ToString()));
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Global.Instance.App_Activated();
        }
    }
}
