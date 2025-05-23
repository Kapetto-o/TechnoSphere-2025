using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.customer;
using TechnoSphere_2025.modules.shared;

namespace TechnoSphere_2025.controls
{
    /// <summary>
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
        }

        private void ProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is DependencyObject src)
            {
                var parentButton = FindAncestor<Button>(src);
                if (parentButton != null)
                {
                    if (!ReferenceEquals(parentButton, sender))
                    {
                        return;
                    }
                }
            }

            if (DataContext is not ProductViewModel vm)
                return;

            var productId = vm.ProductID;
            var nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new PageProduct(productId));
        }

        private void AddFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel vm)
                vm.ToggleFavoriteCommand.Execute(null);

        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel vm)
            {
                int userId = SessionManager.CurrentUserID;
                if (vm.IsInComparison)
                    ComparisonRepository.Remove(userId, vm.ProductID);
                else
                    ComparisonRepository.Add(userId, vm.ProductID);
            }
        }

        private void AddToBasketButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel vm)
            {
                if (vm.IsInBasket)
                {
                    var nav = NavigationService.GetNavigationService(this);
                    nav?.Navigate(new PageBasket());
                }
                else
                {
                    vm.AddToBasketCommand.Execute(null);
                }
            }
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T found)
                    return found;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
