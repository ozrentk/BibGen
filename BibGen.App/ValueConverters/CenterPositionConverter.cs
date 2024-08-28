using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace BibGen.App.ValueConverters
{
    // TODO: Remove
    public class CenterPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return 0D;

            if (values[0] is not double canvasWidth)
                return 0D;

            if (values[1] is not double elementWidth)
                return 0D;

            Debug.WriteLine($"Returning left position: {(double)((canvasWidth - elementWidth) / 2)}");
            return (double)((canvasWidth - elementWidth) / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
