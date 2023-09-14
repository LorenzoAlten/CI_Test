using System.Windows.Controls;

namespace PackDataViewer.Views
{
    /// <summary>
    /// Interaction logic for PackDataView.xaml
    /// </summary>
    public partial class PackDataView : UserControl
    {
        public static PackDataView Instance { get; private set; }

        public PackDataView()
        {
            InitializeComponent();

            Instance = this;
        }
    }
}
