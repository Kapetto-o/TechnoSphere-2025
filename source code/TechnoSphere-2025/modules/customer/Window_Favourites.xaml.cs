using System.Windows;
using System.Windows.Input;

namespace TechnoSphere_2025.modules.customer
{
    /// <summary>
    /// Логика взаимодействия для Window_Favourites.xaml
    /// </summary>
    public partial class Window_Favourites : Window
    {
        public Window_Favourites()
        {
            InitializeComponent();
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
