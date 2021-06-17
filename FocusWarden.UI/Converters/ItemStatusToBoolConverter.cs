namespace FocusWarden.UI.Converters
{
    using Common.Enumerators;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ItemStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not TodoItemStatus status
                ? DependencyProperty.UnsetValue
                : status.ToString().Equals(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isChecked)
            {
                return DependencyProperty.UnsetValue;
            }

            var status =
                (TodoItemStatus)Enum.Parse(typeof(TodoItemStatus), parameter as string ?? string.Empty);

            if (status == TodoItemStatus.Open)
            {
                return isChecked ? TodoItemStatus.Open : TodoItemStatus.Closed;
            }

            return isChecked ? TodoItemStatus.Closed : TodoItemStatus.Open;
        }
    }
}