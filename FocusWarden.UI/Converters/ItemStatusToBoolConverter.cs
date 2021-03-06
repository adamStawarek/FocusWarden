using FocusWarden.Common.Enumerators;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FocusWarden.UI.Converters
{
    public class ItemStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TodoItemStatus status)) return DependencyProperty.UnsetValue;

            return status.ToString().Equals(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool isChecked)) return DependencyProperty.UnsetValue;

            var status = (TodoItemStatus)Enum.Parse(typeof(TodoItemStatus), parameter as string);

            if (status == TodoItemStatus.Open)
            {
                return isChecked ? TodoItemStatus.Open : TodoItemStatus.Closed;
            }
            else
            {
                return isChecked ? TodoItemStatus.Closed : TodoItemStatus.Open;
            }
        }
    }
}
