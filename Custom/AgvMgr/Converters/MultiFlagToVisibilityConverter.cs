using mSwAgilogDll.SEW;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AgvMgr.Converters
{
    public class MultiFlagToVisibilityConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Agv_Station_Status)
            {
                Agv_Station_Status State = (Agv_Station_Status)value[0];

                if (!(value[1] is Agv_Station_Status))
                    return Visibility.Collapsed;
                Agv_Station_Status Flag = (Agv_Station_Status)value[1];

                if (State.HasFlag(Flag))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
