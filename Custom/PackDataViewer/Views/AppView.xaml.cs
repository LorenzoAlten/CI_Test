using mSwDllWPFUtils;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PackDataViewer.Views
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
            vm.OnSnackMessageRequested += Vm_OnSnackMessageRequested;

            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent, new RoutedEventHandler(GotFocus_Event), true);

            Topmost = true;
            Activate();
            Topmost = false;
        }

        private async void Vm_OnSnackMessageRequested(object sender, mSwDllUtils.GenericEventArgs e)
        {
            var messageQueue = MainSnackbar.MessageQueue;

            await Task.Factory.StartNew(() => messageQueue.Enqueue(e.Argument.ToString()));
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Global.Instance.App_Activated();
        }

        private static void GotFocus_Event(object sender, RoutedEventArgs e)
        {
            TextBox control = e.Source as TextBox;
            if (control == null) return;

            control.SelectAll();
        }
    }
}
