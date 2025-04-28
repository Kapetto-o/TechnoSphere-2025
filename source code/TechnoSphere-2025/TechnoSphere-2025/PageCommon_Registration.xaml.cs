using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageCommon_Registration.xaml
    /// </summary>
    public partial class PageCommon_Registration : Page
    {
        public PageCommon_Registration()
        {
            InitializeComponent();
        }
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageCommon_Authorization());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageCustomer_Home());
        }
    }
}
