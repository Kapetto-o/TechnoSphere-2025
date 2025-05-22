using System.Globalization;
using System.Windows.Data;

namespace TechnoSphere_2025.converters
{
    public class BoolToCompareTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? "убрать" : "сравнить";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}