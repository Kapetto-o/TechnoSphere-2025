using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegistrationPage());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePageCustomer());
        }
    }
}
