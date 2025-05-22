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
            // DataContext у нас — ProductViewModel
            if (DataContext is ProductViewModel pvm)
            {
                int userId = SessionManager.CurrentUserID;
                int productId = pvm.ProductID;

                // Удаляем из БД и поднимаем событие ComparisonChanged
                ComparisonRepository.Remove(userId, productId);
            }
        }
    }
}
