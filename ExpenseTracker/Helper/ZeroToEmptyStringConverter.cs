using System.Globalization;

namespace ExpenseTracker.Helper
{
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue && decimalValue == 0)
                return string.Empty;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return decimal.TryParse(value?.ToString(), out var result) ? result : 0;
        }
    }

}
