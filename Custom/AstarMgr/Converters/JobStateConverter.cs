using mSwAgilogDll.MFC.Astar;
using mSwDllWPFUtils;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AstarMgr.Converters
{
    [ValueConversion(sourceType: typeof(EJobStates), targetType: typeof(string))]
    public class JobStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as EJobStates?;
            if (!state.HasValue) return string.Empty;

            switch (state.Value)
            {
                case EJobStates.Stopped:
                    return Global.Instance.LangTl("Stopped");
                case EJobStates.Starting:
                    return Global.Instance.LangTl("Starting");
                case EJobStates.Started:
                    return Global.Instance.LangTl("Started");
                case EJobStates.Running:
                    return Global.Instance.LangTl("Running");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
