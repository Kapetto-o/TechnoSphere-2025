using System.Globalization;
using System.Windows.Data;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.converters
{
    public class SpecValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not SpecificationType specType) return "";
            if (parameter == null || !int.TryParse(parameter.ToString(), out var prodId))
                return "";

            var specs = SpecificationRepository
                            .GetSpecificationsForProduct(prodId)
                            .FirstOrDefault(s => s.SpecTypeID == specType.SpecTypeID);
            if (specs == null) return "";

            return LocalizationManager.CurrentLanguage == LanguageType.Russian
                ? specs.Value_Ru
                : specs.Value_Eng;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
