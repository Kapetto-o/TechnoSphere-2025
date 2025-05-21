using System.Globalization;
using System.Windows.Data;

namespace TechnoSphere_2025.converters
{
    public class BoolToInBasketTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? "Уже в корзине"
                : "В корзину";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}