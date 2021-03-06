using FocusWarden.Common.Enumerators;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FocusWarden.UI.Converters
{
    public class NumericConditionToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 4)
                return DependencyProperty.UnsetValue;
            if (values[0] is int intVal)
                values[0] = (double)intVal;
            if (!(values[0] is double val))
                return DependencyProperty.UnsetValue;
            if (!(double.TryParse(values[1] as string, out var conditionVal)))
                return DependencyProperty.UnsetValue;
            if (!(int.TryParse(values[2] as string, out var precision)))
                return DependencyProperty.UnsetValue;
            if (!Enum.TryParse(values[3] as string, out BooleanConditionType conditionType))
                return DependencyProperty.UnsetValue;

            return conditionType switch
            {
                BooleanConditionType.Equal => Eq(Math.Round(val - conditionVal, precision), 0),
                BooleanConditionType.NotEqual => !Eq(Math.Round(val - conditionVal, precision), 0),
                BooleanConditionType.GreaterThan => Math.Round(val - conditionVal, precision) > 0,
                BooleanConditionType.GreaterThanOrEqual => Math.Round(val - conditionVal, precision) >= 0,
                BooleanConditionType.SmallerThan => Math.Round(val - conditionVal, precision) < 0,
                BooleanConditionType.SmallerThanOrEqual => Math.Round(val - conditionVal, precision) <= 0,
                _ => DependencyProperty.UnsetValue
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool Eq(double d1, double d2, double eps = float.Epsilon)
        {
            return Math.Abs(d1 - d2) < eps;
        }
    }
}
