using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public static class ComparisonRepository
    {
        public static event EventHandler<ComparisonChangedEventArgs>? ComparisonChanged;
        private static string Conn => ConfigurationManager
            .ConnectionStrings["TechnoSphereBD"]!.ConnectionString;

        public static void Add(int userId, int productId)
        {
            const string sql = "insert into Comparisons(UserID,ProductID) values(@u,@p)";
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
            ComparisonChanged?.Invoke(null, new ComparisonChangedEventArgs(userId, productId, true));
        }

        public static void Remove(int userId, int productId)
        {
            const string sql = "delete from Comparisons where UserID=@u and ProductID=@p";
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
            ComparisonChanged?.Invoke(null, new ComparisonChangedEventArgs(userId, productId, false));
        }

        public static bool IsInComparison(int userId, int productId)
        {
            const string sql = "select count(1) from Comparisons where UserID=@u and ProductID=@p";
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            return (int)cmd.ExecuteScalar()! > 0;
        }

        public static List<Product> GetAll(int userId)
        {
            const string sql = @"
                select p.ProductID,p.SKU,p.Name_Ru,p.Name_Eng,
                       p.MainImagePath,p.Description_Ru,p.Description_Eng,
                       p.Price,p.InstallmentPrice,p.PromoPrice,
                       p.StockQuantity,p.IsActive,p.CategoryID
                  from Products p
                  join Comparisons c on c.ProductID=p.ProductID
                 where c.UserID=@u and p.IsActive=1
              order by c.AddedAt";
            var list = new List<Product>();
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Product
                {
                    ProductID = rdr.GetInt32(0),
                    SKU = rdr.GetString(1),
                    Name_Ru = rdr.GetString(2),
                    Name_Eng = rdr.GetString(3),
                    MainImagePath = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                    Price = rdr.GetDecimal(7),
                    InstallmentPrice = rdr.IsDBNull(8) ? null : rdr.GetDecimal(8),
                    PromoPrice = rdr.IsDBNull(9) ? null : rdr.GetDecimal(9),
                    StockQuantity = rdr.GetInt32(10),
                    IsActive = rdr.GetBoolean(11),
                    CategoryID = rdr.GetInt32(12)
                });
            }
            return list;
        }

        public static void ClearCategory(int userId, int categoryId)
        {
            const string sql = @"
                delete c
                  from Comparisons c
                  join Products p on p.ProductID = c.ProductID
                 where c.UserID = @u and p.CategoryID = @cat";
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@cat", categoryId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}