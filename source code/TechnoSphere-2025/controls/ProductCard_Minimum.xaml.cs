using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.modules.shared;

namespace TechnoSphere_2025.controls
{
    /// <summary>
    /// Логика взаимодействия для ProductCard_Minimum.xaml
    /// </summary>
    public partial class ProductCard_Minimum : UserControl, INotifyPropertyChanged
    {
        public ProductCard_Minimum()
        {
            InitializeComponent();

            IncrementCommand = new RelayCommand(_ => Quantity++, _ => true);
            DecrementCommand = new RelayCommand(_ => Quantity--, _ => Quantity > 0);
        }

        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register(
                nameof(Quantity),
                typeof(int),
                typeof(ProductCard_Minimum),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnQuantityChanged));

        public int Quantity
        {
            get => (int)GetValue(QuantityProperty);
            set => SetValue(QuantityProperty, value);
        }

        private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ProductCard_Minimum)d;
            ctl.PropertyChanged?.Invoke(ctl, new PropertyChangedEventArgs(nameof(Quantity)));

            (ctl.DecrementCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not int productId)
                return;

            var nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new PageProduct(productId));
        }
    }
}
