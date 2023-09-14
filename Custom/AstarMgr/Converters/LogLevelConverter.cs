using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AstarMgr.Converters
{
    [ValueConversion(sourceType: typeof(LogLevels), targetType: typeof(string))]
    public class LogLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var level = value as LogLevels?;
            if (!level.HasValue) return string.Empty;

            switch (level.Value)
            {
                case LogLevels.System:
                    return Global.Instance.LangTl("System");
                case LogLevels.Event:
                    return Global.Instance.LangTl("Event");
                case LogLevels.Warning:
                    return Global.Instance.LangTl("Warning");
                case LogLevels.Fatal:
                    return Global.Instance.LangTl("Fatal");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
