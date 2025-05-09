using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.controls.header
{
    /// <summary>
    /// Логика взаимодействия для HeaderControlBase.xaml
    /// </summary>
    public partial class HeaderControlBase : UserControl
    {
        private PopupManager? _popupManager;
        public FrameworkElement LogoButton => Logotype;
        public FrameworkElement UserButton => UserAccount;

        public HeaderControlBase()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            Loaded += HeaderControl_User_Loaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ownerWindow = Window.GetWindow(this);
            if (ownerWindow != null)
            {
                _popupManager = new PopupManager(ownerWindow);
                _popupManager.Register(AccountPopup, UserAccount);
            }
        }

        private void HeaderControl_User_Loaded(object sender, RoutedEventArgs e)
        {
            AccountName.Text = SessionManager.CurrentUsername;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _popupManager?.Unregister();
            _popupManager = null;
        }

        private void Logotype_Click(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            if (SessionManager.CurrentUserRole == 1)
                nav?.Navigate(new PageHome_Admin());
            else
                nav?.Navigate(new PageHome_User());

            e.Handled = true;
        }

        private void UserAccount_Click(object sender, RoutedEventArgs e)
        {
            double desiredWidth = UserAccount.ActualWidth + 22;
            _popupManager?.Toggle(AccountPopup, desiredWidth);
            e.Handled = true;
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Window_Settings();
            if (Window.GetWindow(this) is Window owner)
                settingsWindow.Owner = owner;
            settingsWindow.Show();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Window_AboutApplication();
            if (Window.GetWindow(this) is Window owner)
                aboutWindow.Owner = owner;
            aboutWindow.Show();
        }

        private void CabinetMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tokenString = Properties.Settings.Default.RememberToken;
            if (Guid.TryParse(tokenString, out var token))
            {
                var connString = ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"].ConnectionString;
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(
                        "UPDATE Users SET RememberToken = NULL WHERE RememberToken = @t", conn))
                    {
                        cmd.Parameters.AddWithValue("@t", token);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Properties.Settings.Default.RememberToken = string.Empty;
            Properties.Settings.Default.Save();

            var nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new PageAuthorization());

            if (nav != null)
            {
                while (nav.CanGoBack)
                    nav.RemoveBackEntry();
            }
        }

        public class DelegateCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            public DelegateCommand(Action<T> execute) => _execute = execute;
            public bool CanExecute(object? parameter) => true;
            public void Execute(object? parameter) => _execute((T)parameter!);
            public event EventHandler? CanExecuteChanged;
        }
    }
}
