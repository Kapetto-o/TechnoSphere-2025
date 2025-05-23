using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.modules.admin
{
    /// <summary>
    /// Логика взаимодействия для Window_AddProduct.xaml
    /// </summary>
    public partial class Window_AddProduct : Window
    {
        private readonly int _categoryId;
        // Список типов характеристик для данной категории:
        private List<SpecificationType> _specTypes = new List<SpecificationType>();

        // Словарь, где ключ – SpecTypeID, а значение – TextBox для ввода Value_Ru
        private Dictionary<int, TextBox> _specInputs = new Dictionary<int, TextBox>();

        // Строка подключения
        private static string Conn => ConfigurationManager
            .ConnectionStrings["TechnoSphereBD"].ConnectionString;

        public Window_AddProduct(int categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;
            LoadSpecificationTypes();
            BuildSpecificationsInputs();
        }

        /// <summary>
        /// Загружает из БД список типов характеристик (SpecificationTypes) для данной категории.
        /// </summary>
        private void LoadSpecificationTypes()
        {
            _specTypes.Clear();
            using var conn = new SqlConnection(Conn);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT SpecTypeID, CategoryID, Name_Ru, Name_Eng, IsMain
                  FROM SpecificationTypes
                 WHERE CategoryID = @catId
                 ORDER BY SortOrder", conn);
            cmd.Parameters.AddWithValue("@catId", _categoryId);

            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                _specTypes.Add(new SpecificationType
                {
                    SpecTypeID = rdr.GetInt32(0),
                    CategoryID = rdr.GetInt32(1),
                    Name_Ru = rdr.GetString(2),
                    Name_Eng = rdr.GetString(3),
                    IsMain = rdr.GetBoolean(4)
                });
            }
        }

        /// <summary>
        /// Для каждого SpecType создаёт в UI строку с меткой и TextBox-ом, 
        /// в который админ введёт значение характеристики (Value_Ru).
        /// </summary>
        private void BuildSpecificationsInputs()
        {
            SpecsStackPanel.Children.Clear();
            _specInputs.Clear();

            foreach (var spec in _specTypes)
            {
                var label = LocalizationManager.CurrentLanguage == LanguageType.Russian
                    ? spec.Name_Ru
                    : spec.Name_Eng;

                var row = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                // Метка
                row.Children.Add(new TextBlock
                {
                    Text = label,
                    Style = (Style)FindResource("MainlineColor_2_1"),
                    Margin = new Thickness(0, 0, 0, 2)
                });

                // TextBox для ввода значения характеристики
                var tb = new TextBox
                {
                    Style = (Style)FindResource("TextBoxColor"),
                    Height = 28
                };

                row.Children.Add(tb);

                SpecsStackPanel.Children.Add(row);

                // Сохраняем в словарь связь SpecTypeID → соответствующий TextBox
                _specInputs[spec.SpecTypeID] = tb;
            }
        }

        /// <summary>
        /// Нажатие «Сохранить». Валидирует поля, вставляет товар + характеристики в БД.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1) Считываем и валидируем общие поля
            string nameRu = NameRuTextBox.Text.Trim();
            string nameEn = NameEngTextBox.Text.Trim();
            string imagePath = ImagePathTextBox.Text.Trim();
            string priceStr = PriceTextBox.Text.Trim();
            string promoStr = PromoPriceTextBox.Text.Trim();
            string stockStr = StockQuantityTextBox.Text.Trim();

            if (string.IsNullOrEmpty(nameRu))
            {
                MessageBox.Show("Название (рус.) не может быть пустым.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceStr.Replace(',', '.'),
                                  System.Globalization.NumberStyles.Number,
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  out var price) || price < 0m)
            {
                MessageBox.Show("Некорректная базовая цена.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal? promoPrice = null;
            if (!string.IsNullOrEmpty(promoStr))
            {
                if (!decimal.TryParse(promoStr.Replace(',', '.'),
                                      System.Globalization.NumberStyles.Number,
                                      System.Globalization.CultureInfo.InvariantCulture,
                                      out var tmpPromo) || tmpPromo < 0m)
                {
                    MessageBox.Show("Некорректная акционная цена.", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                promoPrice = tmpPromo;
            }

            if (!int.TryParse(stockStr, out var stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Некорректное количество на складе.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2) Считываем характеристики
            var specValues = new List<(int SpecTypeID, string Value_Ru)>();
            foreach (var kvp in _specInputs)
            {
                var specTypeId = kvp.Key;
                var tb = kvp.Value;
                var valRu = tb.Text.Trim();

                // Пусть пустое значение тоже считается допустимым (если у вас обязательно нужно, 
                // поставьте здесь проверку non-empty)
                specValues.Add((specTypeId, valRu));
            }

            // 3) Вставляем товар в таблицу Products
            int newProductId;
            using (var conn = new SqlConnection(Conn))
            {
                conn.Open();
                using var txn = conn.BeginTransaction();

                try
                {
                    // Генерируем какой-нибудь уникальный SKU, например GUID
                    string sku = Guid.NewGuid().ToString();

                    var cmdInsert = new SqlCommand(@"
                        INSERT INTO Products (SKU, Name_Ru, Name_Eng, MainImagePath,
                                              Price, InstallmentPrice, PromoPrice,
                                              StockQuantity, IsActive, CategoryID)
                        VALUES (@sku, @nameRu, @nameEn, @img,
                                @price, NULL, @promo,
                                @stock, 1, @catId);
                        SELECT SCOPE_IDENTITY();", conn, txn);
                    cmdInsert.Parameters.AddWithValue("@sku", sku);
                    cmdInsert.Parameters.AddWithValue("@nameRu", nameRu);
                    cmdInsert.Parameters.AddWithValue("@nameEn", nameEn);
                    cmdInsert.Parameters.AddWithValue("@img", string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath);
                    cmdInsert.Parameters.AddWithValue("@price", price);
                    cmdInsert.Parameters.AddWithValue("@promo", promoPrice.HasValue ? (object)promoPrice.Value : DBNull.Value);
                    cmdInsert.Parameters.AddWithValue("@stock", stockQuantity);
                    cmdInsert.Parameters.AddWithValue("@catId", _categoryId);

                    // Получаем ID вставленного товара
                    var result = cmdInsert.ExecuteScalar();
                    newProductId = Convert.ToInt32(result);

                    // 4) Вставляем характеристики в таблицу ProductSpecifications
                    foreach (var (specTypeId, valRu) in specValues)
                    {
                        var cmdSpec = new SqlCommand(@"
                            INSERT INTO ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
                            VALUES (@pid, @stid, @vru, @veng);", conn, txn);
                        cmdSpec.Parameters.AddWithValue("@pid", newProductId);
                        cmdSpec.Parameters.AddWithValue("@stid", specTypeId);
                        cmdSpec.Parameters.AddWithValue("@vru", valRu);
                        // Для простоты мы дублируем русское значение в ENG:
                        cmdSpec.Parameters.AddWithValue("@veng", valRu);

                        cmdSpec.ExecuteNonQuery();
                    }

                    // Если нужно: вставить описание, дополнительные поля и т.п.

                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    MessageBox.Show($"Ошибка при добавлении товара:\n{ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Успешно добавили товар
            MessageBox.Show("Товар успешно добавлен.", "Готово",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Закрываем окно
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}