using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace eStation.Ext
{
    /// <summary>
    /// 
    /// </summary>
    public class TabSizeConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var tabControl = values[0] as TabControl;
            var width = (tabControl?.ActualWidth / tabControl?.Items.Count)-5;            
            return (width <= 1) ? 0 : (width - 1);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}









//<extention:TabSizeConverter x:Key="TabSizeConverter" />
       
//        <Style TargetType = "{x:Type TabItem}" >
//            < Setter Property="Width">
//                <Setter.Value>
//                    <MultiBinding Converter = "{StaticResource TabSizeConverter}" >
//                        < Binding RelativeSource="{RelativeSource Mode=FindAncestor, AncestorType={x:Type TabControl}}" />
                          
//                        <Binding RelativeSource = "{RelativeSource Mode=FindAncestor, AncestorType={x:Type TabControl}}" Path="ActualWidth" />
                
//                    </MultiBinding>
//                </Setter.Value>
//            </Setter>
//        </Style>