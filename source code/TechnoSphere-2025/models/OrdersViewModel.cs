using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class OrderStatus
    {
        public int StatusID { get; set; }
        public string Name_Ru { get; set; } = "";
    }

    public class OrderViewModel : INotifyPropertyChanged
    {
        public string StatusName { get; set; } = "";

        public ObservableCollection<OrderItemViewModel> Items { get; }
            = new ObservableCollection<OrderItemViewModel>();

        public int OrderID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; } = "";
        public decimal TotalAmount { get; set; }

        private int _statusId;
        public int StatusID
        {
            get => _statusId;
            set
            {
                if (_statusId == value) return;
                _statusId = value;
                OnPropertyChanged(nameof(StatusID));
                OnPropertyChanged(nameof(StatusName));
                OnPropertyChanged(nameof(CanCancel));
            }
        }

        private ICommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(_ =>
        {
            // найти ID статуса "Отменён" в глобальном справочнике
            var vmRoot = (App.Current.MainWindow?.DataContext as OrdersViewModel);
            var cancelled = vmRoot?.Statuses
                .FirstOrDefault(s => s.Name_Ru.Equals("Отменён", StringComparison.OrdinalIgnoreCase));
            if (cancelled == null)
            {
                MessageBox.Show("Статус 'Отменён' не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // обновляем в БД
            OrdersRepository.UpdateOrderStatus(OrderID, cancelled.StatusID);

            // и локально
            StatusID = cancelled.StatusID;
            StatusName = cancelled.Name_Ru;
        }, _ => CanCancel);

        public bool CanCancel => StatusID == 1;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class OrdersViewModel
    {
        public ObservableCollection<OrderViewModel> Orders { get; }
            = new ObservableCollection<OrderViewModel>();

        public ObservableCollection<OrderStatus> Statuses { get; }
            = new ObservableCollection<OrderStatus>();

        public OrdersViewModel()
        {
            LoadStatuses();
            LoadOrders();
            LoadOrderItems();
        }

        private void LoadStatuses()
        {
            const string sql = @"
        SELECT StatusID, Name_Ru
          FROM OrderStatuses
         ORDER BY StatusID";
            using var conn = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"]!.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Statuses.Add(new OrderStatus
                {
                    // tinyint в SQL → byte в .NET
                    StatusID = (int)rdr.GetByte(0),
                    Name_Ru = rdr.GetString(1)
                });
            }
        }

        private void LoadOrderItems()
        {
            const string sql = @"
        SELECT oi.OrderID, oi.ProductID, p.Name_Ru, oi.Quantity, oi.UnitPrice
          FROM OrderItems oi
          JOIN Products p ON p.ProductID = oi.ProductID";

            using var conn = new SqlConnection(ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]!.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            var lookup = Orders.ToDictionary(o => o.OrderID);

            while (rdr.Read())
            {
                int oid = rdr.GetInt32(0);
                if (!lookup.TryGetValue(oid, out var ovm)) continue;

                ovm.Items.Add(new OrderItemViewModel
                {
                    ProductID = rdr.GetInt32(1),
                    Name = rdr.GetString(2),
                    Quantity = rdr.GetInt32(3),
                    UnitPrice = rdr.GetDecimal(4)
                });
            }
        }

        private void LoadOrders()
        {
            const string sql = @"
        SELECT
            o.OrderID,
            o.CreatedAt,
            u.Username,
            o.StatusID,
            s.Name_Ru AS StatusName,
            SUM(oi.Quantity * oi.UnitPrice) AS TotalAmount
        FROM Orders o
        JOIN Users u ON o.UserID = u.UserID
        JOIN OrderStatuses s ON o.StatusID = s.StatusID
        LEFT JOIN OrderItems oi ON oi.OrderID = o.OrderID
        GROUP BY
            o.OrderID, o.CreatedAt, u.Username, o.StatusID, s.Name_Ru
        ORDER BY o.CreatedAt DESC";

            using var conn = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"]!.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Orders.Add(new OrderViewModel
                {
                    OrderID = rdr.GetInt32(0),
                    CreatedAt = rdr.GetDateTime(1),
                    Username = rdr.GetString(2),
                    StatusID = rdr.GetByte(3),
                    StatusName = rdr.GetString(4),
                    TotalAmount = rdr.GetDecimal(5)
                });
            }
        }
    }

    public static class OrdersRepository
    {
        public static void UpdateOrderStatus(int orderId, int statusId)
        {
            const string sql = @"
        UPDATE Orders
           SET StatusID = @s
         WHERE OrderID = @o";
            using var conn = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"]!.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@s", statusId);
            cmd.Parameters.AddWithValue("@o", orderId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public class OrderItemViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string Display => $"{Quantity} × {Name} ({UnitPrice:N2} р.)";
    }
}