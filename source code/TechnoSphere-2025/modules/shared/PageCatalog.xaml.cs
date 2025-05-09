using System.Windows.Controls;

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
            CategoryText.Text = $"Каталог для категории ID: {_categoryId}";
        }
    }
}
