using Microsoft.Data.SqlClient;
using System.Configuration;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public static class CatalogRepository
    {
        private static string Conn => ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]!.ConnectionString;

        public static string GetCategoryName(int categoryId)
        {
            const string sql = @"SELECT Name_Ru, Name_Eng FROM Categories WHERE CategoryID = @c";
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", categoryId);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return categoryId.ToString();
            return LocalizationManager.CurrentLanguage == LanguageType.Russian
                ? rdr.GetString(0)
                : rdr.GetString(1);
        }
    }
}