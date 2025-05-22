using System.Globalization;
using System.Windows.Data;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.converters
{
    /// <summary>
    /// MultiValueConverter для ячеек таблицы сравнения.
    /// Принимает два значения: 
    ///   values[0] = SpecificationType, 
    ///   values[1] = ProductID (int).
    /// Возвращает строку (Value_Ru или Value_Eng).
    /// </summary>
    public class SpecValueMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return string.Empty;

            var specType = values[0] as SpecificationType;
            if (specType == null)
                return string.Empty;

            if (!(values[1] is int prodId))
                return string.Empty;

            var specs = SpecificationRepository
                            .GetSpecificationsForProduct(prodId)
                            .FirstOrDefault(s => s.SpecTypeID == specType.SpecTypeID);
            if (specs == null)
                return string.Empty;

            return LocalizationManager.CurrentLanguage == LanguageType.Russian
                ? specs.Value_Ru
                : specs.Value_Eng;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}