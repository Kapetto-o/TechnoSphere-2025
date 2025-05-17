using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TechnoSphere_2025.converters
{
    public class ImagePathToSourceConverter : IValueConverter
    {
        private static readonly string Fallback = "images/system-no_image.jpg";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? relPath = value as string;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string file = !string.IsNullOrWhiteSpace(relPath)
                          ? Path.Combine(baseDir, relPath.TrimStart('/', '\\'))
                          : null!;

            if (string.IsNullOrWhiteSpace(relPath) || !File.Exists(file))
            {
                file = Path.Combine(baseDir, Fallback);
            }

            try
            {
                return new BitmapImage(new Uri(file, UriKind.Absolute));
            }
            catch
            {
                return new BitmapImage(new Uri(Path.Combine(baseDir, Fallback), UriKind.Absolute));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}