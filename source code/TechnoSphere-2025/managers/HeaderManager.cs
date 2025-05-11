using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace TechnoSphere_2025.managers
{
    public class HeaderManager : IDisposable
    {
        private readonly PopupManager _popupManager;
        private readonly Window _ownerWindow;
        private readonly Image? _burgerImage;
        private readonly double _burgerSize;
        private readonly double _crossSize;

        /// <summary>
        /// Если вы поддерживаете бургер-меню (CatalogPopup), передаём сюда Image + размеры.
        /// Если нет—передаём null.
        /// </summary>
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
            var settings = new Window_Settings();
            settings.Owner = _ownerWindow;
            settings.Show();
        }

        public void ShowAbout()
        {
            var about = new Window_AboutApplication();
            about.Owner = _ownerWindow;
            about.Show();
        }

        // ----- Exit -----
        public void LogoutAndNavigateToAuth(DependencyObject context)
        {
            // — сброс токена в БД, как было
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

            // 2) Берём NavigationService от переданного элемента
            var nav = NavigationService.GetNavigationService(context);
            if (nav != null)
            {
                nav.Navigate(new PageAuthorization());
                while (nav.CanGoBack)
                    nav.RemoveBackEntry();
            }
        }

        public void Dispose() => _popupManager.Dispose();
    }
}
