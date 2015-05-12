using System;
using System.Globalization;
using System.Windows.Data;

namespace EStation.Ext
{
    /// <summary>
    /// 
    /// </summary>
    public class TabItemWidthAdjustmentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lTabControlWidth = (Double?) value ?? 50; // 50 just to see something, in case of error
            var lTabsCount = (parameter != null && parameter is string) ? int.Parse((string) parameter) : 1;
            return (lTabControlWidth-5) / lTabsCount;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}



//var tabControl = values[0] as TabControl;
//var width = (tabControl?.ActualWidth - 5) / tabControl?.Items.Count ;            
//return (width <= 1) ? 0 : (width - 1);


//var lTabControlWidth = (Double?)value ?? 50; // 50 just to see something, in case of error
//var lTabsCount = (parameter != null && parameter is string) ? int.Parse((string)parameter) : 1;
//            return (lTabControlWidth-5) / lTabsCount;