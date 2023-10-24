using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PolicyManager.Utils.Converters;

public class BooleanVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return parameter != null && parameter.ToString() == "Invert"
            ? value is true
                ? Visibility.Collapsed
                : Visibility.Visible
            : value is true
                ? Visibility.Visible
                : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return parameter != null && parameter.ToString() == "Invert"
            ? value is Visibility.Collapsed
            : value is Visibility.Visible;
    }
}