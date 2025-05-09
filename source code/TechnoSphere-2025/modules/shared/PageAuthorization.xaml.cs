using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.Properties;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для PageAuthorization.xaml
    /// </summary>
    public partial class PageAuthorization : Page
    {
        public PageAuthorization()
        {
            InitializeComponent();
            Loaded += PageAuthorization_Loaded;
        }

        private void PageAuthorization_Loaded(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService;
            while (nav != null && nav.CanGoBack)
                nav.RemoveBackEntry();
        }

        private void ClearErrors()
        {
            LogEmailError.Text = "";
            LogPasswordError.Text = "";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            var model = new UserLoginModel
            {
                Email = LogEmailTextBox.Text.Trim(),
                Password = LogPasswordBox.Password
            };

            var errors = ValidationManager.Validate(model);
            if (errors.Any())
            {
                foreach (var err in errors)
                {
                    var member = err.MemberNames.First();
                    switch (member)
                    {
                        case nameof(model.Email):
                            LogEmailError.Text = err.ErrorMessage;
                            break;
                        case nameof(model.Password):
                            LogPasswordError.Text = err.ErrorMessage;
                            break;
                    }
                }
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["TechnoSphereBD"].ConnectionString;

            byte[] storedHash;
            byte[] storedSalt;
            byte role;
            string dbUsername = string.Empty;

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                SELECT Username, PasswordHash, PasswordSalt, Role
                  FROM Users
                 WHERE Email = @e AND IsActive = 1", conn))
                {
                    cmd.Parameters.AddWithValue("@e", model.Email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            LogEmailError.Text = ErrorValidation.ErrorUserNotFound;
                            return;
                        }

                        int idx = reader.GetOrdinal("Username");
                        dbUsername = reader.IsDBNull(idx)
                            ? string.Empty
                            : reader.GetString(idx);
                        storedHash = (byte[])reader["PasswordHash"];
                        storedSalt = (byte[])reader["PasswordSalt"];
                        role = (byte)reader["Role"];
                    }
                }

                byte[] inputHash;
                using (var sha = SHA256.Create())
                {
                    var combined = Encoding.UTF8.GetBytes(model.Password)
                                     .Concat(storedSalt)
                                     .ToArray();
                    inputHash = sha.ComputeHash(combined);
                }

                if (!inputHash.SequenceEqual(storedHash))
                {
                    LogPasswordError.Text = ErrorValidation.ErrorIncorrectPassword;
                    return;
                }

                var rememberToken = Guid.NewGuid();
                using (var updateCmd = new SqlCommand(
                    "UPDATE Users SET RememberToken = @t WHERE Email = @e", conn))
                {
                    updateCmd.Parameters.AddWithValue("@t", rememberToken);
                    updateCmd.Parameters.AddWithValue("@e", model.Email);
                    updateCmd.ExecuteNonQuery();
                }

                Properties.Settings.Default.RememberToken = rememberToken.ToString();
                Properties.Settings.Default.Save();
            }

            SessionManager.CurrentUsername = dbUsername;
            SessionManager.CurrentUserRole = role;

            if (role == 1)
                NavigationService?.Navigate(new PageHome_Admin());
            else
                NavigationService?.Navigate(new PageHome_User());
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageRegistration());
        }
    }
}