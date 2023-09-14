using System;
using mSwDllWPFUtils;
using System.Globalization;
using System.Windows.Data;

namespace AstarMgr.Converters
{
    [ValueConversion(sourceType: typeof(int), targetType: typeof(string))]
    public class TimeoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeout = value as int?;
            if (!timeout.HasValue) return string.Empty;

            var seconds = (timeout % 60);
            var minutes = (timeout / 60);
            var hours = (timeout / 3600);

            string retVal = string.Empty;

            if (hours > 0) retVal += $", {hours} {Global.Instance.LangTl("Hours")}";
            if (minutes > 0) retVal += $", {minutes} {Global.Instance.LangTl("Min")}";
            if (seconds > 0) retVal += $", {seconds} {Global.Instance.LangTl("Sec")}";

            return retVal.Length > 0 ? retVal.Remove(0, 2).Trim() : retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
