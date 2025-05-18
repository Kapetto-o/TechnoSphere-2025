using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.modules.customer
{
    /// <summary>
    /// Логика взаимодействия для Window_Favourites.xaml
    /// </summary>
    public partial class Window_Favourites : Window
    {
        private readonly FavoritesViewModel _vm;

        public Window_Favourites()
        {
            InitializeComponent();
            _vm = new FavoritesViewModel();
            DataContext = _vm;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
