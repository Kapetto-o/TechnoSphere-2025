using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            DataContext = this;

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
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?> _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
