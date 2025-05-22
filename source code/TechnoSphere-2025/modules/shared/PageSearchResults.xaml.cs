using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TechnoSphere_2025.controls.header;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.modules.shared
{
    /// <summary>
    /// Логика взаимодействия для PageSearchResults.xaml
    /// </summary>
    public partial class PageSearchResults : Page
    {
        public ObservableCollection<ProductViewModel> Products { get; } = new ObservableCollection<ProductViewModel>();

        public PageSearchResults(string query)
        {
            InitializeComponent();
            LoadHeader();
            DataContext = new SearchResultsViewModel(query);
        }

        private void LoadHeader()
        {
            HeaderHost.Content = SessionManager.CurrentUserRole == 1
                ? (UIElement)new HeaderControl_Admin()
                : new HeaderControl_User();
        }
    }
}
