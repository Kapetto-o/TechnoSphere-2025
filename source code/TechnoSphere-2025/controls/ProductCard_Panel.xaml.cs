using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.controls
{
    /// <summary>
    /// Логика взаимодействия для ProductCard_Panel.xaml
    /// </summary>
    public partial class ProductCard_Panel : UserControl
    {
        public ProductCard_Panel()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel pvm)
            {
                int userId = SessionManager.CurrentUserID;
                int productId = pvm.ProductID;

                ComparisonRepository.Remove(userId, productId);
            }
        }
    }
}
