using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.controls.header
{
    public partial class HeaderControl_User : UserControl
    {
        private const double BurgerIconSize = 25;
        private const double CrossIconSize = 20;
        private PopupManager? _popupManager;

        public HeaderControl_User()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void BurgerCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            if (_popupManager == null)
            {
                var ownerWindow = Window.GetWindow(this);
                _popupManager = new PopupManager(ownerWindow);
                _popupManager.Register(CatalogPopup, BurgerCatalogButton);
            }

            _popupManager.Toggle(CatalogPopup);
        }

        private void CatalogPopup_Opened(object sender, EventArgs e)
        {
            SetBurgerIcon(isCross: true);
        }

        private void CatalogPopup_Closed(object sender, EventArgs e)
        {
            SetBurgerIcon(isCross: false);
        }

        private void SetBurgerIcon(bool isCross)
        {
            var key = isCross ? "CrossIcon" : "BurgerIcon";
            BurgerCatalogImage.SetResourceReference(
                Image.SourceProperty,
                key
            );

            BurgerCatalogImage.Width = isCross ? CrossIconSize : BurgerIconSize;
            BurgerCatalogImage.Height = isCross ? CrossIconSize : BurgerIconSize;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ownerWindow = Window.GetWindow(this);
            if (ownerWindow != null)
            {
                _popupManager = new PopupManager(ownerWindow);
                _popupManager.Register(CatalogPopup, BurgerCatalogButton);
            }
            UpdateBurgerIcon(CatalogPopup.IsOpen);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _popupManager?.Unregister();
            _popupManager = null;
        }

        private void UpdateBurgerIcon(bool isCross)
        {
            var key = isCross ? "CrossIcon" : "BurgerIcon";
            BurgerCatalogImage.SetResourceReference(
                Image.SourceProperty,
                key
            );
            BurgerCatalogImage.Width = isCross ? CrossIconSize : BurgerIconSize;
            BurgerCatalogImage.Height = isCross ? CrossIconSize : BurgerIconSize;
        }

    }
}
