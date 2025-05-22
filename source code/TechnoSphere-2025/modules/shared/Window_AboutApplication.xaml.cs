using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для Window_AboutApplication.xaml
    /// </summary>
    public partial class Window_AboutApplication : Window
    {
        public Window_AboutApplication()
        {
            InitializeComponent();
        }

        private void BrowserLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);

            e.Handled = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
