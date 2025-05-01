using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using TechnoSphere_2025.helper;
using TechnoSphere_2025.helper.validation;

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

            string connString = ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]
                .ConnectionString;

            byte[] storedHash;
            byte[] storedSalt;
            byte role;

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    SELECT PasswordHash, PasswordSalt, Role
                      FROM Users
                     WHERE Email = @e AND IsActive = 1", conn))
                {
                    cmd.Parameters.AddWithValue("@e", model.Email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            LogEmailError.Text = "Пользователь не найден или заблокирован";
                            return;
                        }

                        // Считываем все поля до закрытия reader
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
                    LogPasswordError.Text = "Неверный пароль";
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

                if (role == 1)
                    NavigationService?.Navigate(new PageHome_Admin());
                else
                    NavigationService?.Navigate(new PageHome_User());
            }
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageRegistration());
        }
    }
}