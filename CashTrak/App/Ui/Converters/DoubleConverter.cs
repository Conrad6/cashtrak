using System;
using System.Globalization;
using System.Windows.Data;

namespace CashTrak.App.Ui.Converters
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue)) return null;
            return double.TryParse(stringValue, out var ans) ? new double?(ans) : null;
        }
    }
}