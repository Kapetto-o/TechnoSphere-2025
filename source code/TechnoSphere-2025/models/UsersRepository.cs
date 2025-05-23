using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public class UserData
    {
        public int UserID { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }

    public static class UsersRepository
    {
        private static string ConnStr =>
            ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]!
                .ConnectionString;

        public static List<UserData> GetAllUsers()
        {
            const string sql = @"
        SELECT UserID, Username, Email, FirstName, LastName, Phone, IsActive
          FROM Users
         WHERE Role = 0
         ORDER BY Username";

            var list = new List<UserData>();
            using var conn = new SqlConnection(ConnStr);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new UserData
                {
                    UserID = rdr.GetInt32(0),
                    Username = rdr.GetString(1),
                    Email = rdr.GetString(2),
                    FirstName = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                    LastName = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                    Phone = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                    IsActive = rdr.GetBoolean(6)
                });
            }
            return list;
        }

        public static void SetActive(int userId, bool isActive)
        {
            const string sql = @"
                UPDATE Users
                   SET IsActive = @a
                 WHERE UserID = @u";
            using var conn = new SqlConnection(ConnStr);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@a", isActive);
            cmd.Parameters.AddWithValue("@u", userId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
