using System.Windows.Controls;
using System.Windows.Input;

namespace OrdersMgr.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }

        private void Selection_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            ((AgilogDll.MisOrder)checkBox.DataContext).IsSelected = !checkBox.IsChecked.Value;
            ((ViewModels.OrdersViewModel)DataContext).RefreshSelectedOrders();

            e.Handled = true;
        }
    }
}
