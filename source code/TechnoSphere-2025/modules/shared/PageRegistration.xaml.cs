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
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
            Loaded += PageHome_Loaded;
        }

        private void PageHome_Loaded(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService;
            while (nav != null && nav.CanGoBack)
                nav.RemoveBackEntry();
        }

        private void ClearErrors()
        {
            RegUsernameError.Text = "";
            RegEmailError.Text = "";
            RegPasswordError.Text = "";
            RegRepeatPasswordError.Text = "";
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            var model = new UserRegistrationModel
            {
                Username = RegUsernameTextBox.Text.Trim(),
                Email = RegEmailTextBox.Text.Trim(),
                Password = RegPasswordBox.Password,
                RepeatPassword = RegRepeatPasswordBox.Password
            };

            var errors = ValidationManager.Validate(model);
            if (errors.Any())
            {
                foreach (var err in errors)
                {
                    var member = err.MemberNames.FirstOrDefault();
                    switch (member)
                    {
                        case nameof(model.Username):
                            RegUsernameError.Text = err.ErrorMessage;
                            break;
                        case nameof(model.Email):
                            RegEmailError.Text = err.ErrorMessage;
                            break;
                        case nameof(model.Password):
                            RegPasswordError.Text = err.ErrorMessage;
                            break;
                        case nameof(model.RepeatPassword):
                            RegRepeatPasswordError.Text = err.ErrorMessage;
                            break;
                    }
                }
                return;
            }

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            byte[] hash;
            using (var sha = SHA256.Create())
            {
                var combined = Encoding.UTF8.GetBytes(model.Password)
                                 .Concat(salt)
                                 .ToArray();
                hash = sha.ComputeHash(combined);
            }

            var rememberToken = Guid.NewGuid();

            string connString = ConfigurationManager.ConnectionStrings["TechnoSphereBD"].ConnectionString;

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var checkUser = new SqlCommand(
                    "SELECT COUNT(1) FROM Users WHERE Username = @u", conn))
                {
                    checkUser.Parameters.AddWithValue("@u", model.Username);
                    if ((int)checkUser.ExecuteScalar() > 0)
                    {
                        RegUsernameError.Text = ErrorValidation.ErrorUsernameExists;
                        return;
                    }
                }

                using (var checkEmail = new SqlCommand(
                    "SELECT COUNT(1) FROM Users WHERE Email = @e", conn))
                {
                    checkEmail.Parameters.AddWithValue("@e", model.Email);
                    if ((int)checkEmail.ExecuteScalar() > 0)
                    {
                        RegEmailError.Text = ErrorValidation.ErrorEmailExists;
                        return;
                    }
                }

                using (var insert = new SqlCommand(@"
                    INSERT INTO Users
                        (Username, Email, PasswordHash, PasswordSalt, Role, IsActive, RememberToken)
                    VALUES
                        (@u, @e, @ph, @ps, 0, 1, @t)", conn))
                {
                    insert.Parameters.AddWithValue("@u", model.Username);
                    insert.Parameters.AddWithValue("@e", model.Email);
                    insert.Parameters.AddWithValue("@ph", hash);
                    insert.Parameters.AddWithValue("@ps", salt);
                    insert.Parameters.AddWithValue("@t", rememberToken);

                    insert.ExecuteNonQuery();
                }
            }

            Properties.Settings.Default.RememberToken = rememberToken.ToString();
            Properties.Settings.Default.Save();

            SessionManager.CurrentUsername = model.Username;
            SessionManager.RememberToken = rememberToken;

            NavigationService?.Navigate(new PageHome_User());
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageAuthorization());
        }
    }
}