using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.modules.admin
{
    /// <summary>
    /// Логика взаимодействия для PageUsers.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        public PageUsers()
        {
            InitializeComponent();
            LoadHeader();
        }

        private void LoadHeader()
        {
            HeaderHost.Content = SessionManager.CurrentUserRole == 1
                ? (UIElement)new HeaderControl_Admin()
                : new HeaderControl_User();
        }
    }
}
