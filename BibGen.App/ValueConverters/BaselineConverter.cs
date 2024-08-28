using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace BibGen.App.ValueConverters
{
    // TODO: Remove
    public class BaselineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return 0D;

            if (values[0] is not decimal baseline)
                return 0D;

            if (values[1] is not double controlHeight)
                return 0D;

            Debug.WriteLine($"Returning top position: {(double)baseline * controlHeight}");
            return (double)baseline * controlHeight;
        }

        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
