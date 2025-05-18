using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using TechnoSphere_2025.modules.customer;

namespace TechnoSphere_2025.managers
{
    public class HeaderManager : IDisposable
    {
        private readonly PopupManager _popupManager;
        private readonly Window _ownerWindow;
        private readonly Image? _burgerImage;
        private readonly double _burgerSize;
        private readonly double _crossSize;

        public HeaderManager(
            Window ownerWindow,
            Image? burgerImage = null,
            double burgerIconSize = 25,
            double crossIconSize = 20)
        {
            _ownerWindow = ownerWindow
                ?? throw new ArgumentNullException(nameof(ownerWindow));
            _popupManager = new PopupManager(ownerWindow);
            _burgerImage = burgerImage;
            _burgerSize = burgerIconSize;
            _crossSize = crossIconSize;
        }

        // ----- CatalogPopup -----
        public void RegisterCatalog(Popup popup, UIElement owner)
            => _popupManager.Register(popup, owner);

        public void ToggleCatalog(Popup popup)
            => _popupManager.Toggle(popup);

        public void OnCatalogOpened()
            => SetBurgerIcon(isCross: true);

        public void OnCatalogClosed()
            => SetBurgerIcon(isCross: false);

        private void SetBurgerIcon(bool isCross)
        {
            if (_burgerImage == null) return;
            var key = isCross ? "CrossIcon" : "BurgerIcon";
            _burgerImage.SetResourceReference(Image.SourceProperty, key);

            _burgerImage.Width = isCross ? _crossSize : _burgerSize;
            _burgerImage.Height = isCross ? _crossSize : _burgerSize;
        }

        // ----- AccountPopup -----
        public void RegisterAccount(Popup popup, UIElement owner)
            => _popupManager.Register(popup, owner);

        public void ToggleAccount(Popup popup, double? width = null)
            => _popupManager.Toggle(popup, width);

        // ----- Logotype -----
        public void NavigateHome(DependencyObject context)
        {
            var nav = NavigationService.GetNavigationService(context);
            if (nav == null) return;

            if (SessionManager.CurrentUserRole == 1)
                nav.Navigate(new PageHome_Admin());
            else
                nav.Navigate(new PageHome_User());
        }

        // ----- Settings / About -----
        public void ShowSettings()
        {
            var existing = Application.Current.Windows
            .OfType<Window_Settings>()
            .FirstOrDefault(w => w.Owner == _ownerWindow);

            if (existing != null)
            {
                if (existing.WindowState == WindowState.Minimized)
                    existing.WindowState = WindowState.Normal;
                existing.Activate();
            }
            else
            {
                var fav = new Window_Settings
                {
                    Owner = _ownerWindow
                };
                fav.Show();
            }
        }

        public void ShowAbout()
        {
            var existing = Application.Current.Windows
            .OfType<Window_AboutApplication>()
            .FirstOrDefault(w => w.Owner == _ownerWindow);

            if (existing != null)
            {
                if (existing.WindowState == WindowState.Minimized)
                    existing.WindowState = WindowState.Normal;
                existing.Activate();
            }
            else
            {
                var fav = new Window_AboutApplication
                {
                    Owner = _ownerWindow
                };
                fav.Show();
            }
        }

        // ----- Exit -----
        public void LogoutAndNavigateToAuth(DependencyObject context)
        {
            var tokenString = Properties.Settings.Default.RememberToken;
            if (Guid.TryParse(tokenString, out var token))
            {
                var connString = ConfigurationManager
                    .ConnectionStrings["TechnoSphereBD"]
                    .ConnectionString;
                using var conn = new SqlConnection(connString);
                conn.Open();
                using var cmd = new SqlCommand(
                    "UPDATE Users SET RememberToken = NULL WHERE RememberToken = @t",
                    conn);
                cmd.Parameters.AddWithValue("@t", token);
                cmd.ExecuteNonQuery();
            }

            Properties.Settings.Default.RememberToken = "";
            Properties.Settings.Default.Save();

            var nav = NavigationService.GetNavigationService(context);
            if (nav != null)
            {
                nav.Navigate(new PageAuthorization());
                while (nav.CanGoBack)
                    nav.RemoveBackEntry();
            }
        }

        // ----- Menu -----
        public void Favourites()
        {
            var existing = Application.Current.Windows
                .OfType<Window_Favourites>()
                .FirstOrDefault();

            if (existing != null)
            {
                if (existing.WindowState == WindowState.Minimized)
                    existing.WindowState = WindowState.Normal;
                existing.Activate();
            }
            else
            {
                var fav = new Window_Favourites();

                fav.WindowStartupLocation = WindowStartupLocation.Manual;
                fav.Left = _ownerWindow.Left + (_ownerWindow.Width - fav.Width) / 2;
                fav.Top = _ownerWindow.Top + (_ownerWindow.Height - fav.Height) / 2;

                fav.Show();
            }
        }

        public void Dispose() => _popupManager.Dispose();
    }
}