namespace FocusWarden.UI.Converters
{
    using DataAccess.Models;
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;

    public class FilterSettingsToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not FilterSettings filterSettings)
            {
                return DependencyProperty.UnsetValue;
            }

            var header = new StringBuilder();

            if (filterSettings.Status.IsChecked)
            {
                header.Append($"Status: {filterSettings.Status.Value.ToString()}");
            }

            if (filterSettings.CreatedAt.IsChecked)
            {
                if (header.Length > 0)
                {
                    header.Append(" Or");
                }

                header.Append($"\nCreated at: {filterSettings.CreatedAt.Value.ToShortDateString()}");
            }

            if (filterSettings.ClosedAt.IsChecked)
            {
                if (header.Length > 0)
                {
                    header.Append(" Or");
                }

                header.Append($"\nClosed at: {filterSettings.CreatedAt.Value.ToShortDateString()}");
            }

            return header.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}