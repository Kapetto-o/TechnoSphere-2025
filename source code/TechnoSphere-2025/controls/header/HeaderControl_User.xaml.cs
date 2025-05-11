using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.shared;

namespace TechnoSphere_2025.controls.header
{
    public partial class HeaderControl_User : UserControl
    {
        private HeaderManager? _hdr;
        public CatalogViewModel CatalogVm { get; } = new CatalogViewModel();
        public ICommand NavigateCategoryCommand { get; }

        public HeaderControl_User()
        {
            InitializeComponent();
            DataContext = this;

            NavigateCategoryCommand = new DelegateCommand<int>(categoryId =>
            {
                CatalogPopup.IsOpen = false;

                var target = new PageCatalog(categoryId);
                var nav = NavigationService.GetNavigationService(this);
                nav?.Navigate(target);
            });

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var owner = Window.GetWindow(this)!;
            _hdr = new HeaderManager(owner, BurgerCatalogImage);

            _hdr.RegisterCatalog(CatalogPopup, BurgerCatalogButton);

            _hdr.OnCatalogClosed();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _hdr?.Dispose();
            _hdr = null;
        }

        private void BurgerCatalogButton_Click(object sender, RoutedEventArgs e)
            => _hdr?.ToggleCatalog(CatalogPopup);

        private void CatalogPopup_Opened(object sender, EventArgs e)
            => _hdr?.OnCatalogOpened();

        private void CatalogPopup_Closed(object sender, EventArgs e)
            => _hdr?.OnCatalogClosed();
    }
}