using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.modules.shared
{
    /// <summary>
    /// Логика взаимодействия для PageCatalog.xaml
    /// </summary>
    public partial class PageCatalog : Page
    {
        private readonly int _categoryId;

        public PageCatalog(int categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;

            LoadHeader();
            LoadCategory();
        }

        private void LoadHeader()
        {
            HeaderHost.Content = SessionManager.CurrentUserRole == 1
                ? (UIElement)new HeaderControl_Admin()
                : new HeaderControl_User();
        }

        private void LoadCategory()
        {
            var all = LoadAllCategoriesFromDb();
            var lookup = all.ToDictionary(c => c.CategoryID);
            var path = new List<Category>();

            var cur = lookup[_categoryId];
            while (cur != null)
            {
                path.Insert(0, cur);
                if (!cur.ParentCategoryID.HasValue) break;
                cur = lookup[cur.ParentCategoryID.Value];
            }

            BreadcrumbStackPanel.Children.Clear();
            var styleNormal = (Style)FindResource("MainlineColor_2_1");
            var styleCurrent = (Style)FindResource("MainlineColor_2_3");

            BreadcrumbStackPanel.Children.Add(new TextBlock
            {
                Text = "Каталог",
                Style = styleNormal
            });

            foreach (var cat in path)
            {
                BreadcrumbStackPanel.Children.Add(new TextBlock
                {
                    Text = ">",
                    Style = styleNormal,
                    Margin = new Thickness(5, 0, 5, 0)
                });

                var isLast = cat.CategoryID == _categoryId;
                BreadcrumbStackPanel.Children.Add(new TextBlock
                {
                    Text = LocalizationManager.CurrentLanguage == LanguageType.Russian
                           ? cat.Name_Ru
                           : cat.Name_Eng,
                    Style = isLast ? styleCurrent : styleNormal
                });
            }

            var last = path.Last();
            CategoryHeader.Text = LocalizationManager.CurrentLanguage == LanguageType.Russian
                ? last.Name_Ru
                : last.Name_Eng;

            ProductsList.Items.Clear();
            ProductsList.Items.Add($"Товар 1 в категории {_categoryId}");
            ProductsList.Items.Add($"Товар 2 в категории {_categoryId}");
            ProductsList.Items.Add($"Товар 3 в категории {_categoryId}");
        }

        private List<Category> LoadAllCategoriesFromDb()
        {
            var list = new List<Category>();
            var connStr = ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]?.ConnectionString
                ?? throw new ConfigurationErrorsException("ConnectionString не найдена.");

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT CategoryID, ParentCategoryID, Name_Ru, Name_Eng, SortOrder, IsActive
                      FROM Categories
                      WHERE IsActive = 1", conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new Category
                        {
                            CategoryID = rdr.GetInt32(0),
                            ParentCategoryID = rdr.IsDBNull(1) ? null : rdr.GetInt32(1),
                            Name_Ru = rdr.GetString(2),
                            Name_Eng = rdr.GetString(3),
                            SortOrder = rdr.GetInt32(4),
                            IsActive = rdr.GetBoolean(5)
                        });
                    }
                }
            }
            return list;
        }
    }
}