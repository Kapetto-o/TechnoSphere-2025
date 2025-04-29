using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageCommon_Authorization.xaml
    /// </summary>
    public partial class PageCommon_Authorization : Page
    {
        public PageCommon_Authorization()
        {
            InitializeComponent();
        }
        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageCommon_Registration());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageCustomer_Home());
        }
    }
}
