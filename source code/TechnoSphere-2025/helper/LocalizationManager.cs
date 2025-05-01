using System.Globalization;
using System.Windows;

namespace TechnoSphere_2025.helper
{
    public enum LanguageType
    {
        Russian,
        English
    }

    public static class LocalizationManager
    {
        private const string LangFolder = "/languages/";
        private const string FilePattern = "{0}.xaml";

        public static LanguageType CurrentLanguage { get; private set; }

        public static void Initialize()
        {
            var saved = Properties.Settings.Default.Language;
            LanguageType lang;

            if (!string.IsNullOrWhiteSpace(saved) &&
                Enum.TryParse(saved, true, out lang))
            {
            }
            else
            {
                var twoLetter = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                lang = twoLetter.Equals("ru", StringComparison.OrdinalIgnoreCase)
                    ? LanguageType.Russian
                    : LanguageType.English;
            }

            ApplyLanguage(lang, saveIfNeeded: false);
        }

        public static void ApplyLanguage(LanguageType lang, bool saveIfNeeded = false)
        {
            var culture = lang == LanguageType.Russian
                ? new CultureInfo("ru")
                : new CultureInfo("en");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var md = Application.Current.Resources.MergedDictionaries;
            for (int i = md.Count - 1; i >= 0; i--)
            {
                var src = md[i].Source?.OriginalString;
                if (src != null &&
                    src.StartsWith(LangFolder, StringComparison.OrdinalIgnoreCase))
                {
                    md.RemoveAt(i);
                }
            }

            var uri = new Uri($"{LangFolder}{string.Format(FilePattern, lang)}", UriKind.Relative);
            md.Add(new ResourceDictionary { Source = uri });

            md.RemoveAt(md.Count - 1);
            md.Add(new ResourceDictionary { Source = uri });

            CurrentLanguage = lang;
            if (saveIfNeeded)
            {
                Properties.Settings.Default.Language = lang.ToString();
                Properties.Settings.Default.Save();
            }

            ReloadLogoDictionary();
            ThemeManager.ReloadIconDictionary();
        }

        public static void ReloadLogoDictionary()
        {
            var md = Application.Current.Resources.MergedDictionaries;
            var langDict = md.FirstOrDefault(d =>
                d.Source?.OriginalString
                    .StartsWith(LangFolder, StringComparison.OrdinalIgnoreCase) == true);
            if (langDict == null) return;

            var src = langDict.Source;
            md.Remove(langDict);
            md.Add(new ResourceDictionary { Source = src });
        }
    }
}