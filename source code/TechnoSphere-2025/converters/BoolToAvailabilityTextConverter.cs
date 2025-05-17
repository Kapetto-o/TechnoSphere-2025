using System.Globalization;
using System.Windows.Data;

namespace TechnoSphere_2025.converters
{
    public class BoolToAvailabilityTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? "В наличии"
                : "Нет в наличии";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}