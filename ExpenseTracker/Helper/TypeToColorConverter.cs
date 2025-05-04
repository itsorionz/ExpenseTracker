using System.Globalization;


namespace ExpenseTracker.Helper
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value?.ToString()) switch
            {
                "Income" => Colors.Green,
                "Expense" => Colors.Red,
                _ => Colors.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
