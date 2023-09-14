using mSwAgilogDll;
using mSwDllUtils;
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

namespace WhsViewer
{
    /// <summary>
    /// Interaction logic for LocationControl.xaml
    /// </summary>
    public partial class LocationControl : UserControl
    {
        public LocationControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var location = (AgilogDll.WhsCellsLocation)DataContext;
            if (location == null) return;

            location.IsSelected = !location.IsSelected;
            AgilogDll.WarehouseCellMgr.Ptr.RefreshSelection(location);
        }
    }
}
