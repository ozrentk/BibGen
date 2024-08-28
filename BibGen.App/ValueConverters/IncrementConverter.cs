using System.Windows.Data;

namespace BibGen.App.ValueConverters
{
    public class IncrementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not int intValue)
                return value;

            return intValue + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not int intValue)
                return value;

            return intValue - 1;
        }
    }
}
