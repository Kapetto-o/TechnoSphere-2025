using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.controls.header
{
    public partial class HeaderControlBase : UserControl
    {
        private HeaderManager? _hdr;
        public FrameworkElement LogoButton => Logotype;

        public FrameworkElement UserButton => UserAccount;

        public HeaderControlBase()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Loaded += HeaderControlBase_Loaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var owner = Window.GetWindow(this)!;
            _hdr = new HeaderManager(owner);

            _hdr.RegisterAccount(AccountPopup, UserAccount);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _hdr?.Dispose();
            _hdr = null;
        }

        private void Logotype_Click(object sender, RoutedEventArgs e)
        {
            _hdr?.NavigateHome(this);
            e.Handled = true;
        }

        private void UserAccount_Click(object sender, RoutedEventArgs e)
        {
            var width = UserAccount.ActualWidth + 22;
            _hdr?.ToggleAccount(AccountPopup, width);
            e.Handled = true;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _hdr?.LogoutAndNavigateToAuth(this);
            e.Handled = true;
        }


        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
            => _hdr?.ShowSettings();

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
            => _hdr?.ShowAbout();

        private void HeaderControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            AccountName.Text = SessionManager.CurrentUsername;
        }
    }
}
