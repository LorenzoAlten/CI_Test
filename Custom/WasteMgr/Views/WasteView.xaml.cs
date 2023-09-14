using System.Windows;
using System.Windows.Controls;

namespace WasteMgr.Views
{
    /// <summary>
    /// Interaction logic for EntranceView.xaml
    /// </summary>
    public partial class WasteView : UserControl
    {
        public static WasteView Instance { get; protected set; }

        public WasteView()
        {
            InitializeComponent();

            Instance = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
