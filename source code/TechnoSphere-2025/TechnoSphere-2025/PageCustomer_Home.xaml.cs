using System.Windows;
using System.Windows.Controls;

namespace TechnoSphere_2025
{
    public partial class PageCustomer_Home : Page
    {
        private PopupManager? _popupManager;

        public PageCustomer_Home()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                _popupManager = new PopupManager(parentWindow);
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

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutApp = new Window_AboutApplication();
            Window owner = Window.GetWindow(this);
            if (owner != null)
                aboutApp.Owner = owner;
            aboutApp.Show();
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutApp = new Window_Settings();
            Window owner = Window.GetWindow(this);
            if (owner != null)
                aboutApp.Owner = owner;
            aboutApp.Show();
        }
    }
}