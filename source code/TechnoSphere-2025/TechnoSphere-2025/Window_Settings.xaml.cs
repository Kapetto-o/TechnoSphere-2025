using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.helper;

namespace TechnoSphere_2025
{
    public partial class Window_Settings : Window
    {
        public Window_Settings()
        {
            InitializeComponent();

            switch (ThemeManager.CurrentTheme)
            {
                case ThemeType.System:
                    SystemThemeRadioButton.IsChecked = true;
                    break;
                case ThemeType.Light:
                    LightThemeRadioButton.IsChecked = true;
                    break;
                case ThemeType.Dark:
                    DarkThemeRadioButton.IsChecked = true;
                    break;
            }

            SystemThemeRadioButton.Checked += ThemeRadioButton_Checked;
            LightThemeRadioButton.Checked += ThemeRadioButton_Checked;
            DarkThemeRadioButton.Checked += ThemeRadioButton_Checked;
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SystemThemeRadioButton.IsChecked == true)
                ThemeManager.ApplyTheme(ThemeType.System, saveIfNeeded: true);
            else if (LightThemeRadioButton.IsChecked == true)
                ThemeManager.ApplyTheme(ThemeType.Light, saveIfNeeded: true);
            else if (DarkThemeRadioButton.IsChecked == true)
                ThemeManager.ApplyTheme(ThemeType.Dark, saveIfNeeded: true);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}