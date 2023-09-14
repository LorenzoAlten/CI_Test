using System.Windows;
using System.Windows.Controls;

namespace PickMgr.Views
{
    /// <summary>
    /// Interaction logic for EntranceView.xaml
    /// </summary>
    public partial class PickView : UserControl
    {
        public static PickView Instance { get; protected set; }

        public PickView()
        {
            InitializeComponent();

            Instance = this;
        }
    }
}
