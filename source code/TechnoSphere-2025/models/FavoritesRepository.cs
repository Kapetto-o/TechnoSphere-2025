using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public class FavoriteChangedEventArgs : EventArgs
    {
        public int UserID { get; }
        public int ProductID { get; }
        public bool IsAdded { get; }

        public FavoriteChangedEventArgs(int userId, int productId, bool isAdded)
        {
            UserID = userId;
            ProductID = productId;
            IsAdded = isAdded;
        }
    }

    public static class FavoritesRepository
    {
        public static event EventHandler<FavoriteChangedEventArgs>? FavoriteChanged;

        private static string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]!
                .ConnectionString;

        public static bool IsFavorite(int userId, int productId)
        {
            const string sql = @"
                select count(1)
                  from Favorites
                 where UserID = @u
                   and ProductID = @p;
            ";

            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", userId);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();

            var cnt = (int)cmd.ExecuteScalar()!;
            return cnt > 0;
        }

        public static async Task AddFavorite(int userId, int productId)
        {
            const string sql = @"
            insert into Favorites(UserID, ProductID)
            values(@u, @p);
        ";

            await Task.Run(() =>
            {
                using var conn = new SqlConnection(ConnectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", userId);
                cmd.Parameters.AddWithValue("@p", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            });

            FavoriteChanged?.Invoke(
                null,
                new FavoriteChangedEventArgs(userId, productId, isAdded: true));
        }

        public static async Task RemoveFavorite(int userId, int productId)
        {
            const string sql = @"
            delete from Favorites
             where UserID    = @u
               and ProductID = @p;
        ";

            await Task.Run(() =>
            {
                using var conn = new SqlConnection(ConnectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", userId);
                cmd.Parameters.AddWithValue("@p", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            });

            FavoriteChanged?.Invoke(
                null,
                new FavoriteChangedEventArgs(userId, productId, isAdded: false));
        }

        public static List<Product> GetFavorites(int userId)
        {
            const string sql = @"
                select p.ProductID, p.SKU, p.Name_Ru, p.Name_Eng,
                       p.MainImagePath, p.Description_Ru, p.Description_Eng,
                       p.Price, p.InstallmentPrice, p.PromoPrice,
                       p.StockQuantity, p.IsActive, p.CategoryID
                  from Products p
                  join Favorites f
                    on f.ProductID = p.ProductID
                 where f.UserID = @u
                   and p.IsActive = 1;
            ";

            var list = new List<Product>();
            using var conn = new SqlConnection(ConnectionString);
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
    }
}