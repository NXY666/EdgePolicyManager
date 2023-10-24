using System;
using Microsoft.UI.Xaml.Data;

namespace PolicyManager.Utils.Converters;

public class NegationBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is not true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is not true;
    }
}