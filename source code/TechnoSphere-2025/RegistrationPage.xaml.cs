using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthorizationPage());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePageCustomer());
        }
    }
}
