using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
        }
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageAuthorization());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageHome_User());
        }
    }
}
