using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.modules.admin
{
    public partial class PageOrders : Page
    {
        public PageOrders()
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
