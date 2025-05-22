using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class ComparisonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ComparisonTabViewModel> Tabs { get; } = new();
        private ComparisonTabViewModel? _activeTab;
        public ComparisonTabViewModel? ActiveTab
        {
            get => _activeTab;
            set
            {
                if (_activeTab == value) return;
                _activeTab = value;
                OnPropertyChanged(nameof(ActiveTab));
            }
        }

        public ICommand ClearTabCommand { get; }

        public ComparisonViewModel()
        {
            LoadInitialComparisons();

            ClearTabCommand = new DelegateCommand<object>(_ =>
            {
                if (ActiveTab == null)
                    return;

                var tab = ActiveTab;
                int userId = SessionManager.CurrentUserID;
                int categoryId = tab.CategoryID;

                ComparisonRepository.ClearCategory(userId, categoryId);

                tab.Products.Clear();
                tab.SpecTypes.Clear();
                tab.Rows.Clear();

                Tabs.Remove(tab);

                ActiveTab = Tabs.FirstOrDefault(t => t.Products.Any());
            });

            ComparisonRepository.ComparisonChanged += OnComparisonChanged;
        }

        private void LoadInitialComparisons()
        {
            int userId = SessionManager.CurrentUserID;
            var allProductsInComparison = ComparisonRepository.GetAll(userId);

            foreach (var prodModel in allProductsInComparison)
            {
                var pvm = new ProductViewModel(prodModel);
                int catId = pvm.CategoryID;

                var tab = Tabs.FirstOrDefault(t => t.CategoryID == catId);
                if (tab == null)
                {
                    string name = CatalogRepository.GetCategoryName(catId);
                    tab = new ComparisonTabViewModel(catId, name);
                    foreach (var st in SpecificationRepository.GetTypesByCategory(catId))
                        tab.SpecTypes.Add(st);
                    Tabs.Add(tab);
                }

                if (!tab.Products.Any(x => x.ProductID == pvm.ProductID))
                {
                    tab.Products.Add(pvm);
                    pvm.IsInComparison = true;
                }
            }

            foreach (var t in Tabs)
                t.RefreshRows();

            ActiveTab = Tabs.FirstOrDefault(t => t.Products.Any());
        }

        private void OnComparisonChanged(object? sender, ComparisonChangedEventArgs e)
        {
            if (e.UserID != SessionManager.CurrentUserID)
                return;

            var prod = ProductRepository.GetById(e.ProductID);
            var pvm = new ProductViewModel(prod);
            int catId = pvm.CategoryID;

            var tab = Tabs.FirstOrDefault(t => t.CategoryID == catId);

            if (e.IsAdded)
            {
                if (tab == null)
                {
                    string name = CatalogRepository.GetCategoryName(catId);
                    tab = new ComparisonTabViewModel(catId, name);
                    foreach (var st in SpecificationRepository.GetTypesByCategory(catId))
                        tab.SpecTypes.Add(st);
                    Tabs.Add(tab);
                }

                if (!tab.Products.Any(x => x.ProductID == pvm.ProductID))
                {
                    tab.Products.Add(pvm);
                    pvm.IsInComparison = true;
                }

                tab.RefreshRows();

                if (ActiveTab == null || ActiveTab.CategoryID != catId)
                    ActiveTab = tab;
            }
            else
            {
                if (tab != null)
                {
                    var existing = tab.Products.FirstOrDefault(x => x.ProductID == e.ProductID);
                    if (existing != null)
                    {
                        tab.Products.Remove(existing);
                        existing.IsInComparison = false;
                    }

                    tab.RefreshRows();

                    if (!tab.Products.Any())
                    {
                        if (ActiveTab == tab)
                            ActiveTab = Tabs.FirstOrDefault(t2 => t2.Products.Any());
                        Tabs.Remove(tab);
                    }
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}