using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoSphere_2025.models
{
    public static class OrderRepository
    {
        private static string Conn => ConfigurationManager
            .ConnectionStrings["TechnoSphereBD"].ConnectionString;

        public static int CreateOrder(int userId, string contactName, string contactPhone, string deliveryAddress,
                                      IEnumerable<BasketItemData> items)
        {
            using var conn = new SqlConnection(Conn);
            conn.Open();
            using var tx = conn.BeginTransaction();

            const string sqlOrder = @"
            insert into Orders (UserID, StatusID, Delivery, ContactName, ContactPhone)
            output inserted.OrderID
            values (@u, 1, @d, @n, @p);
        ";
            int orderId;
            using (var cmd = new SqlCommand(sqlOrder, conn, tx))
            {
                cmd.Parameters.AddWithValue("@u", userId);
                cmd.Parameters.AddWithValue("@d", deliveryAddress ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@n", contactName);
                cmd.Parameters.AddWithValue("@p", contactPhone);
                orderId = (int)cmd.ExecuteScalar()!;
            }

            const string sqlItem = @"
            insert into OrderItems (OrderID, ProductID, Quantity, UnitPrice)
            values (@o, @p, @q, @price);
        ";
            foreach (var dto in items)
            {
                using var cmd = new SqlCommand(sqlItem, conn, tx);
                cmd.Parameters.AddWithValue("@o", orderId);
                cmd.Parameters.AddWithValue("@p", dto.Product.ProductID);
                cmd.Parameters.AddWithValue("@q", dto.Quantity);
                decimal unitPrice = dto.Product.PromoPrice ?? dto.Product.Price;
                cmd.Parameters.AddWithValue("@price", unitPrice);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return orderId;
        }
    }
}
