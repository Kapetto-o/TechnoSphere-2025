using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageHome_Admin.xaml
    /// </summary>
    public partial class PageHome_User : Page
    {
        public PageHome_User()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JustLoggedIn)
            {
                var nav = NavigationService;
                while (nav != null && nav.CanGoBack)
                    nav.RemoveBackEntry();

                SessionManager.JustLoggedIn = false;
            }
        }
    }
}