using System.Windows.Controls;
using System.Windows;

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
            Loaded += PageHome_User_Loaded;
        }

        private void PageHome_User_Loaded(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService;
            while (nav != null && nav.CanGoBack)
            {
                nav.RemoveBackEntry();
            }
        }
    }
}