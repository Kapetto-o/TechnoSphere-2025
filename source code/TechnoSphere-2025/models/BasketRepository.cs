using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public class BasketItemData
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public static class BasketRepository
    {
        private static string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]!
                .ConnectionString;

        public static List<BasketItemData> GetBasketItems(int userId)
        {
            const string sql = @"
                SELECT 
                    b.ProductID, 
                    b.Quantity,
                    p.SKU, p.Name_Ru, p.Name_Eng, p.MainImagePath,
                    p.Price, p.InstallmentPrice, p.PromoPrice,
                    p.StockQuantity, p.IsActive, p.CategoryID
                FROM BasketItems b
                JOIN Products p ON b.ProductID = p.ProductID
                WHERE b.UserID = @u
                  AND p.IsActive = 1
                ORDER BY b.AddedAt DESC;
            ";

            var result = new List<BasketItemData>();
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", userId);
                conn.Open();
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var prod = new Product
                    {
                        ProductID = rdr.GetInt32(0),
                        SKU = rdr.GetString(2),
                        Name_Ru = rdr.GetString(3),
                        Name_Eng = rdr.GetString(4),
                        MainImagePath = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Price = rdr.GetDecimal(6),
                        InstallmentPrice = rdr.IsDBNull(7) ? null : rdr.GetDecimal(7),
                        PromoPrice = rdr.IsDBNull(8) ? null : rdr.GetDecimal(8),
                        StockQuantity = rdr.GetInt32(9),
                        IsActive = rdr.GetBoolean(10),
                        CategoryID = rdr.GetInt32(11)
                    };

                    int qty = rdr.GetInt32(1);

                    result.Add(new BasketItemData
                    {
                        Product = prod,
                        Quantity = qty
                    });
                }
            }

            return result;
        }

        public static void AddToBasket(int userId, int productId)
        {
            const string sqlCheck = @"
                SELECT Quantity
                  FROM BasketItems
                 WHERE UserID = @u AND ProductID = @p;
            ";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmdCheck = new SqlCommand(sqlCheck, conn))
            {
                cmdCheck.Parameters.AddWithValue("@u", userId);
                cmdCheck.Parameters.AddWithValue("@p", productId);
                conn.Open();
                var existing = cmdCheck.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                {
                    int oldQty = Convert.ToInt32(existing);
                    const string sqlUpd = @"
                        UPDATE BasketItems
                           SET Quantity = @newQty
                         WHERE UserID = @u AND ProductID = @p;
                    ";
                    using var cmdUpd = new SqlCommand(sqlUpd, conn);
                    cmdUpd.Parameters.AddWithValue("@u", userId);
                    cmdUpd.Parameters.AddWithValue("@p", productId);
                    cmdUpd.Parameters.AddWithValue("@newQty", oldQty + 1);
                    cmdUpd.ExecuteNonQuery();
                }
                else
                {
                    const string sqlIns = @"
                        INSERT INTO BasketItems(UserID, ProductID, Quantity)
                        VALUES(@u, @p, 1);
                    ";
                    using var cmdIns = new SqlCommand(sqlIns, conn);
                    cmdIns.Parameters.AddWithValue("@u", userId);
                    cmdIns.Parameters.AddWithValue("@p", productId);
                    cmdIns.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateQuantity(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                RemoveFromBasket(userId, productId);
                return;
            }

            const string sql = @"
                UPDATE BasketItems
                   SET Quantity = @q
                 WHERE UserID = @u AND ProductID = @p;
            ";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            cmd.Parameters.AddWithValue("@q", quantity);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public static void RemoveFromBasket(int userId, int productId)
        {
            const string sql = @"
                DELETE
                  FROM BasketItems
                 WHERE UserID = @u AND ProductID = @p;
            ";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public static void ClearBasket(int userId)
        {
            const string sql = @"
                DELETE
                  FROM BasketItems
                 WHERE UserID = @u;
            ";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public static bool IsInBasket(int userId, int productId)
        {
            const string sql = @"
        SELECT COUNT(1)
          FROM BasketItems
         WHERE UserID = @u
           AND ProductID = @p;
    ";
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            var cnt = (int)cmd.ExecuteScalar()!;
            return cnt > 0;
        }
    }
}