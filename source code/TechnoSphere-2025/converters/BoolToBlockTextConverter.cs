using System.Globalization;
using System.Windows.Data;

namespace TechnoSphere_2025.converters
{
    public class BoolToBlockTextConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
                return isActive ? "Заблокировать" : "Разблокировать";

            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}