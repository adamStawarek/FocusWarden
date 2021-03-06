using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FocusWarden.UI.Converters
{
    public class EnumeratorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum e) return e.ToString();
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
