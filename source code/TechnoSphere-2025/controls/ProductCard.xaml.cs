using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.customer;

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

        private void AddToBasketButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ProductViewModel vm)
                return;

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

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ProductViewModel;
            if (vm == null) return;

            int userId = SessionManager.CurrentUserID;
            if (ComparisonRepository.IsInComparison(userId, vm.ProductID))
                ComparisonRepository.Remove(userId, vm.ProductID);
            else
                ComparisonRepository.Add(userId, vm.ProductID);
        }
    }
}
