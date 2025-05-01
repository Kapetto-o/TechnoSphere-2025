using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TechnoSphere_2025.helper;
using TechnoSphere_2025.helper.validation;
using System.Configuration;

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
                var combined = Encoding.UTF8.GetBytes(model.Password).Concat(salt).ToArray();
                hash = sha.ComputeHash(combined);
            }

            string connString = ConfigurationManager.ConnectionStrings["TechnoSphereBD"].ConnectionString;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO Users
                      (Username, Email, PasswordHash, PasswordSalt, Role, IsActive)
                    VALUES
                      (@u, @e, @ph, @ps, 0, 1)", conn);

                cmd.Parameters.AddWithValue("@u", model.Username);
                cmd.Parameters.AddWithValue("@e", model.Email);
                cmd.Parameters.AddWithValue("@ph", hash);
                cmd.Parameters.AddWithValue("@ps", salt);

                cmd.ExecuteNonQuery();
            }

            NavigationService?.Navigate(new PageHome_User());
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageAuthorization());
        }
    }
}