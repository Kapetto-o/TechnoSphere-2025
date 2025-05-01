using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TechnoSphere_2025.helper
{
    public partial class HeaderControl : UserControl
    {
        private PopupManager? _popupManager;
        public FrameworkElement LogoButton => LogoSetting;
        public FrameworkElement UserButton => UserAccount;

        public HeaderControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ownerWindow = Window.GetWindow(this);
            if (ownerWindow != null)
            {
                _popupManager = new PopupManager(ownerWindow);
                _popupManager.Register(LogoSettingPopup, LogoSetting);
                _popupManager.Register(AccountPopup, UserAccount);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _popupManager?.Unregister();
            _popupManager = null;
        }

        private void LogoSetting_Click(object sender, RoutedEventArgs e)
        {
            _popupManager?.Toggle(LogoSettingPopup);
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
            var nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new PageAuthorization());

            if (nav != null)
            {
                while (nav.CanGoBack)
                    nav.RemoveBackEntry();
            }
        }
    }
}
