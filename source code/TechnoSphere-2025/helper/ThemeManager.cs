using System.Windows;
using Microsoft.Win32;

namespace TechnoSphere_2025.helper
{
    public enum ThemeType
    {
        System,
        Light,
        Dark
    }

    class ThemeManager
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";

        public static ThemeType CurrentTheme { get; private set; }

        public static void Initialize()
        {
            var saved = Properties.Settings.Default.Theme;
            if (!Enum.TryParse(saved, out ThemeType theme))
                theme = ThemeType.System;

            CurrentTheme = theme;
            ApplyTheme(CurrentTheme);
        }

        public static void ApplyTheme(ThemeType theme, bool saveIfNeeded = false)
        {
            var actual = theme == ThemeType.System
                         ? (DetectSystemTheme() ? ThemeType.Light : ThemeType.Dark)
                         : theme;

            var dictUri = new Uri($"/style/themes/{actual}Theme.xaml", UriKind.Relative);

            var md = Application.Current.Resources.MergedDictionaries;
            for (int i = md.Count - 1; i >= 0; i--)
            {
                var d = md[i];
                if (d.Source?.OriginalString.StartsWith("/style/themes/") == true)
                    md.RemoveAt(i);
            }

            md.Insert(0, new ResourceDictionary { Source = dictUri });

            ReloadIconDictionary();
            LocalizationManager.ReloadLogoDictionary();
            CurrentTheme = theme;

            if (saveIfNeeded)
            {
                Properties.Settings.Default.Theme = theme.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private static bool DetectSystemTheme()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath);
                if (key?.GetValue(RegistryValueName) is int val)
                    return val == 1;
            }
            catch { }
            return true;
        }

        public static void ReloadIconDictionary()
        {
            var md = Application.Current.Resources.MergedDictionaries;

            var iconDict = md.FirstOrDefault(d =>
                d.Source != null &&
                d.Source.OriginalString.IndexOf("icons.xaml", StringComparison.OrdinalIgnoreCase) >= 0);

            if (iconDict == null) return;

            var source = iconDict.Source;
            md.Remove(iconDict);
            md.Add(new ResourceDictionary { Source = source });
        }
    }
}