using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FocusWarden.UI.Converters
{
    public class AutoItemSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3) return DependencyProperty.UnsetValue;
            if (values[0] is not int itemsCount) return DependencyProperty.UnsetValue;
            if (values[1] is not double containerHeight) return DependencyProperty.UnsetValue;
            if (values[2] is not double containerWidth) return DependencyProperty.UnsetValue;
            var containerArea = containerWidth * containerHeight;
            var maxItemArea = containerArea / itemsCount;
            var idealItemSize = Math.Sqrt(maxItemArea);
            var nw = Math.Ceiling(containerWidth / idealItemSize);
            var nh = Math.Ceiling(containerHeight / idealItemSize);
            return Math.Min(containerWidth / nw, containerHeight / nh);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
