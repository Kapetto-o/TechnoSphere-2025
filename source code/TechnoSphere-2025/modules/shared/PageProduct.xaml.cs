using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.customer;

namespace TechnoSphere_2025.modules.shared
{
    /// <summary>
    /// Логика взаимодействия для PageProduct.xaml
    /// </summary>
    public partial class PageProduct : Page
    {
        private Product? _product;
        private readonly int _productId;

        private static string Conn => ConfigurationManager
    .ConnectionStrings["TechnoSphereBD"].ConnectionString;

        public PageProduct(int productId)
        {
            InitializeComponent();
            _productId = productId;
            LoadHeader();
            LoadProduct();
            AdjustForRole();
        }

        private void LoadHeader()
        {
            HeaderHost.Content = SessionManager.CurrentUserRole == 1
                ? (UIElement)new HeaderControl_Admin()
                : new HeaderControl_User();
        }

        private void LoadProduct()
        {
            using var connection = new SqlConnection(Conn);
            connection.Open();

            var command = new SqlCommand(@"
        SELECT ProductID, SKU, Name_Ru, Name_Eng, MainImagePath,
               Price, InstallmentPrice, PromoPrice, StockQuantity, IsActive, CategoryID
        FROM Products
        WHERE ProductID = @id", connection);
            command.Parameters.AddWithValue("@id", _productId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                _product = new Product
                {
                    ProductID = reader.GetInt32(0),
                    SKU = reader.GetString(1),
                    Name_Ru = reader.GetString(2),
                    Name_Eng = reader.GetString(3),
                    MainImagePath = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Price = reader.GetDecimal(5),
                    InstallmentPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    PromoPrice = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                    StockQuantity = reader.GetInt32(8),
                    IsActive = reader.GetBoolean(9),
                    CategoryID = reader.GetInt32(10)
                };

                // Преобразуем Product в ViewModel для биндинга
                DataContext = new ProductViewModel(_product);
            }

            LoadSpecifications();
        }


        private void LoadSpecifications()
        {
            using var connection = new SqlConnection(Conn);
            connection.Open();

            var command = new SqlCommand(@"
        SELECT s.Name_Ru, ps.Value_Ru
        FROM ProductSpecifications ps
        JOIN SpecificationTypes s on ps.SpecTypeID = s.SpecTypeID
        WHERE ps.ProductID = @id", connection);
            command.Parameters.AddWithValue("@id", _productId);

            SpecificationEntriesPanel.Children.Clear();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var name = LocalizationManager.CurrentLanguage == LanguageType.Russian
                            ? reader.GetString(0)
                            : reader.GetString(1); // здесь при необходимости выбирайте правильный столбец
                var value = reader.GetString(1);

                var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 3, 0, 0) };
                row.Children.Add(new TextBlock
                {
                    Text = $"{name} ",
                    Style = (Style)FindResource("MainlineColor_2_1")
                });
                row.Children.Add(new TextBlock
                {
                    Text = value,
                    Style = (Style)FindResource("MainlineColor_2_3"),
                    Margin = new Thickness(5, 0, 0, 0)
                });
                SpecificationEntriesPanel.Children.Add(row);
            }
        }

        private void AdjustForRole()
        {
            // Если текущий пользователь – админ (Role == 1), то:
            if (SessionManager.CurrentUserRole == 1)
            {
                // Показываем админские кнопки:
                EditPriceButton.Visibility = Visibility.Visible;
                DeleteProductButton.Visibility = Visibility.Visible;

                // Скрываем/отключаем клиентские:
                AddToBasketButton.Visibility = Visibility.Collapsed;
                CompareButton.Visibility = Visibility.Collapsed;
                AddFavoriteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Для покупателя оставляем всё как есть (админские кнопки останутся скрыты)
                EditPriceButton.Visibility = Visibility.Collapsed;
                DeleteProductButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EditPriceButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ProductViewModel vm)
                return;

            // 1) Ввод базовой цены
            var baseInput = Interaction.InputBox(
                $"Текущая цена: {vm.Price:N2}\nВведите новую базовую цену (число):",
                "Редактирование цены",
                vm.Price.ToString("F2"));
            if (!decimal.TryParse(baseInput.Replace(',', '.'),
                                  System.Globalization.NumberStyles.Number,
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  out var newBasePrice))
            {
                MessageBox.Show("Некорректный ввод базовой цены. Операция отменена.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2) Ввод акционной цены (можно оставить пустым для отмены акционной)
            string defaultPromo = vm.PromoPrice.HasValue ? vm.PromoPrice.Value.ToString("F2") : "";
            var promoInput = Interaction.InputBox(
                $"Текущая акционная цена: {(vm.PromoPrice.HasValue ? vm.PromoPrice.Value.ToString("N2") : "<нет>")}\n" +
                "Введите новую акционную цену или оставьте пустым, чтобы убрать её:",
                "Редактирование акционной цены",
                defaultPromo);

            decimal? newPromoPrice = null;
            if (!string.IsNullOrWhiteSpace(promoInput))
            {
                if (decimal.TryParse(promoInput.Replace(',', '.'),
                                     System.Globalization.NumberStyles.Number,
                                     System.Globalization.CultureInfo.InvariantCulture,
                                     out var parsedPromo))
                {
                    newPromoPrice = parsedPromo;
                }
                else
                {
                    MessageBox.Show("Некорректный ввод акционной цены. Операция отменена.",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // 3) Применяем изменения в БД
            try
            {
                ProductRepository.UpdatePrices(vm.ProductID, newBasePrice, newPromoPrice);
                vm.SetPrices(newBasePrice, newPromoPrice);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить новые цены:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ProductViewModel vm) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить товар «{vm.Name}» (ID={vm.ProductID})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                ProductRepository.Delete(vm.ProductID);

                // Навигация обратно в каталог, например:
                var nav = NavigationService.GetNavigationService(this);
                nav?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось удалить товар:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void AddFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ProductViewModel vm)
                return;

            vm.ToggleFavoriteCommand.Execute(null);
        }
    }
}
