using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.shared;

namespace TechnoSphere_2025.modules.customer
{
    /// <summary>
    /// Логика взаимодействия для PagePersonalAccount.xaml
    /// </summary>
    public partial class PagePersonalAccount : Page
    {
        public PagePersonalAccount()
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

        private void OrderItem_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink hl && hl.Tag is int pid)
            {
                NavigationService?.Navigate(new PageProduct(pid));
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not OrdersViewModel ovm) return;
            if (sender is Button btn && btn.Tag is int orderId)
            {
                // находим в справочнике статус "Отменён" (Name_Ru = "Отменён")
                var cancelledStatus = ovm.Statuses
                    .FirstOrDefault(s => s.Name_Ru.Equals("Отменён", StringComparison.OrdinalIgnoreCase));
                if (cancelledStatus == null)
                {
                    MessageBox.Show("Не удалось найти статус 'Отменён' в справочнике.", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // сохраняем в базе
                OrdersRepository.UpdateOrderStatus(orderId, cancelledStatus.StatusID);

                // обновляем отображение
                var orderVm = ovm.Orders.FirstOrDefault(o => o.OrderID == orderId);
                if (orderVm != null)
                {
                    orderVm.StatusID = cancelledStatus.StatusID;
                    orderVm.StatusName = cancelledStatus.Name_Ru;
                }
            }
        }
    }
}
