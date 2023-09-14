using mSwAgilogDll;
using System;
using System.Globalization;
using System.Windows.Data;

namespace OrdersMgr
{
    public class OrderMissionsCompletedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null)
            {
                return string.Empty;
            }

            if ((OrdPhaseType)values[0] != OrdPhaseType.SCH) return string.Empty;
            if (!int.TryParse(values[1].ToString(), out int completed)) return string.Empty;
            if (!int.TryParse(values[2].ToString(), out int total)) return string.Empty;

            return $"{completed}/{total}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderLineMissionsCompletedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null)
            {
                return string.Empty;
            }

            if ((OrdDetStateType)values[0] != OrdDetStateType.E) return string.Empty;
            if (!int.TryParse(values[1].ToString(), out int completed)) return string.Empty;
            if (!int.TryParse(values[2].ToString(), out int total)) return string.Empty;

            return $"{completed}/{total}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
