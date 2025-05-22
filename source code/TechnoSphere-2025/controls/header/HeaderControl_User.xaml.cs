using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.customer;
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            RunSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RunSearch();
            }
        }

        private void RunSearch()
        {
            var q = SearchTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(q))
                return;

            CatalogPopup.IsOpen = false;

            var nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new PageSearchResults(q));
        }

        private void BurgerCatalogButton_Click(object sender, RoutedEventArgs e)
            => _hdr?.ToggleCatalog(CatalogPopup);

        private void CatalogPopup_Opened(object sender, EventArgs e)
            => _hdr?.OnCatalogOpened();

        private void CatalogPopup_Closed(object sender, EventArgs e)
            => _hdr?.OnCatalogClosed();

        private void OpenFavouritesButton_Click(object sender, RoutedEventArgs e)
            => _hdr?.Favourites();

        private void OpenBasketButton_Click(object sender, RoutedEventArgs e)
            => _hdr?.Basket(this);

        private void OpenComparisonButton_Click(object sender, RoutedEventArgs e)
        {
            var existing = Application.Current.Windows
                .OfType<Window_Comparison>()
                .FirstOrDefault(w => w.Owner == Window.GetWindow(this));
            if (existing != null)
            {
                if (existing.WindowState == WindowState.Minimized)
                    existing.WindowState = WindowState.Normal;
                existing.Activate();
            }
            else
            {
                var win = new Window_Comparison
                {
                    Owner = Window.GetWindow(this)
                };
                win.Show();
            }
        }

    }
}