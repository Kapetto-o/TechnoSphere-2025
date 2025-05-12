using System.Windows.Controls;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;

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
            if (SessionManager.CurrentUserRole == 1)
            {
                HeaderHost.Content = new HeaderControl_Admin();
            }
            else
            {
                HeaderHost.Content = new HeaderControl_User();
            }
        }

        private void LoadCategory()
        {
            //CategoryHeader.Text = $"Категория #{_categoryId}";
            ProductsList.Items.Clear();
            ProductsList.Items.Add($"Товар 1 в категории {_categoryId}");
            ProductsList.Items.Add($"Товар 2 в категории {_categoryId}");
            ProductsList.Items.Add($"Товар 3 в категории {_categoryId}");
        }
        //<TextBlock x:Name="CategoryHeader"
        //               FontSize="24"
        //               FontWeight="Bold"
        //               HorizontalAlignment="Center"
        //               VerticalAlignment="Top"
        //               Margin="0,20,0,0"
        //               Text="Категория"/>
    }
}