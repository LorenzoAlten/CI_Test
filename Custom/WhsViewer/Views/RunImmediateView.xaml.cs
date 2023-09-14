using MaterialDesignThemes.Wpf;
using mSwDllUtils;
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

namespace WhsViewer.Views
{
    /// <summary>
    /// Interaction logic for RunImmediateView.xaml
    /// </summary>
    public partial class RunImmediateView : UserControl
    {
        public RunImmediateView()
        {
            InitializeComponent();

            foreach (TextBox text in this.FindLogicalChildren<TextBox>())
            {
                text.GotFocus += Text_GotFocus;
            }
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
