using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.modules.admin;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageHome_User.xaml
    /// </summary>
    public partial class PageHome_Admin : Page
    {
        public PageHome_Admin()
        {
            InitializeComponent();
        }

        private void OpenUsers_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Frame)Application.Current
                .MainWindow
                .FindName("MainFrame");
            frame.Navigate(new PageUsers());
        }
    }
}