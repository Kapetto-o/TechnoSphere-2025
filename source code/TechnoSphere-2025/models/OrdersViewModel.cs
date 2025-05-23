using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TechnoSphere_2025.models
{
    public class OrderStatus
    {
        public int StatusID { get; set; }
        public string Name_Ru { get; set; } = "";
    }

    public class OrderViewModel : INotifyPropertyChanged
    {
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
                // При изменении статуса — пишем в БД
                OrdersRepository.UpdateOrderStatus(OrderID, _statusId);
            }
        }

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


        private void LoadOrders()
{
    const string sql = @"
        SELECT
            o.OrderID,
            o.CreatedAt,
            u.Username,
            SUM(oi.Quantity * oi.UnitPrice) AS TotalAmount,
            o.StatusID
          FROM Orders o
          JOIN Users u
            ON o.UserID = u.UserID
          LEFT JOIN OrderItems oi
            ON oi.OrderID = o.OrderID
         GROUP BY
            o.OrderID, o.CreatedAt, u.Username, o.StatusID
         ORDER BY
            o.CreatedAt DESC;
    ";

            using var conn = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"]!.ConnectionString);
            using var cmd  = new SqlCommand(sql, conn);
    conn.Open();
    using var rdr  = cmd.ExecuteReader();
    while (rdr.Read())
    {
        Orders.Add(new OrderViewModel
        {
            OrderID     = rdr.GetInt32(0),
            CreatedAt   = rdr.GetDateTime(1),
            Username    = rdr.GetString(2),
            TotalAmount = rdr.GetDecimal(3),     // <- теперь сумма из позиций
            StatusID    = rdr.GetByte(4) /* или GetInt32(4) если StatusID tinyint */,
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
}