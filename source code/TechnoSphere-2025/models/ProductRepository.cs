using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public static class ProductRepository
    {
        private static string Conn => ConfigurationManager
            .ConnectionStrings["TechnoSphereBD"].ConnectionString;

        public static List<Product> GetAllActiveProducts()
        {
            const string sql = @"
            select ProductID, SKU, Name_Ru, Name_Eng,
                   MainImagePath, Description_Ru, Description_Eng,
                   Price, InstallmentPrice, PromoPrice,
                   StockQuantity, IsActive, CategoryID
              from Products
             where IsActive = 1;";
            var list = new List<Product>();
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
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
