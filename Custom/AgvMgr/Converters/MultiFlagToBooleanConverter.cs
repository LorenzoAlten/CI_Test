using mSwAgilogDll;
using mSwAgilogDll.SEW;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AgvMgr.Converters
{
    public class MultiFlagToBooleanConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is SEW_State_Flags)
            {
                SEW_State_Flags State = (SEW_State_Flags)value[0];

                if (!(value[1] is SEW_State_Flags))
                    return false;
                SEW_State_Flags Flag = (SEW_State_Flags)value[1];

                return State.HasFlag(Flag);
            }
            if (value[0] is State_Flags)
            {
                State_Flags State = (State_Flags)value[0];

                if (!(value[1] is State_Flags))
                    return false;
                State_Flags Flag = (State_Flags)value[1];

                return State.HasFlag(Flag);
            }
            if (value[0] is Agv_Station_Status)
            {
                Agv_Station_Status State = (Agv_Station_Status)value[0];

                if (!(value[1] is Agv_Station_Status))
                    return false;
                Agv_Station_Status Flag = (Agv_Station_Status)value[1];

                return State.HasFlag(Flag);
            }
            return false;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
