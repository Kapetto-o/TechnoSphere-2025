using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public static class SpecificationRepository
    {
        private static string Conn => ConfigurationManager
            .ConnectionStrings["TechnoSphereBD"].ConnectionString;

        public static List<SpecificationType> GetTypesByCategory(int categoryId)
        {
            const string sql = @"
            select SpecTypeID, CategoryID, Name_Ru, Name_Eng, IsMain
              from SpecificationTypes
             where CategoryID = @c
             order by SortOrder";
            var list = new List<SpecificationType>();
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", categoryId);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new SpecificationType
                {
                    SpecTypeID = rdr.GetInt32(0),
                    CategoryID = rdr.GetInt32(1),
                    Name_Ru = rdr.GetString(2),
                    Name_Eng = rdr.GetString(3),
                    IsMain = rdr.GetBoolean(4)
                });
            return list;
        }

        public static List<ProductSpecification> GetSpecificationsForProduct(int productId)
        {
            const string sql = @"
            select ProductID, SpecTypeID, Value_Ru, Value_Eng
              from ProductSpecifications
             where ProductID = @p";
            var list = new List<ProductSpecification>();
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@p", productId);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new ProductSpecification
                {
                    ProductID = rdr.GetInt32(0),
                    SpecTypeID = rdr.GetInt32(1),
                    Value_Ru = rdr.GetString(2),
                    Value_Eng = rdr.GetString(3)
                });
            return list;
        }
    }
}
