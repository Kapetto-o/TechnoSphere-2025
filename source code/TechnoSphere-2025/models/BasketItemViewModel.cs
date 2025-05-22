using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class BasketItemViewModel : INotifyPropertyChanged, IDisposable
    {
        private int _quantity;
        private readonly ProductViewModel _productVm;

        public int ProductID => _productVm.ProductID;
        public ProductViewModel ProductVm => _productVm;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value == _quantity) return;
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice));
                (DecrementCommand as RelayCommand)?.RaiseCanExecuteChanged();

                try
                {
                    int userId = SessionManager.CurrentUserID;
                    BasketRepository.UpdateQuantity(userId, ProductID, _quantity);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось обновить количество в корзине:\n{ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                QuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public decimal TotalPrice
        {
            get
            {
                decimal unitPrice = ProductVm.PromoPrice ?? ProductVm.Price;
                return unitPrice * Quantity;
            }
        }

        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand RemoveCommand { get; }

        public event EventHandler? QuantityChanged;

        public BasketItemViewModel(ProductViewModel productVm, int initialQuantity)
        {
            _productVm = productVm;
            _quantity = initialQuantity;

            IncrementCommand = new RelayCommand(_ =>
            {
                Quantity++;
            }, _ => true);

            DecrementCommand = new RelayCommand(_ =>
            {
                Quantity--;
            }, _ => Quantity > 1);

            RemoveCommand = new RelayCommand(_ =>
            {
                try
                {
                    int userId = SessionManager.CurrentUserID;
                    BasketRepository.RemoveFromBasket(userId, ProductID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось удалить позицию из корзины:\n{ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ItemRemoved?.Invoke(this, EventArgs.Empty);
            }, _ => true);
        }

        public event EventHandler? ItemRemoved;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        public void Dispose()
        {
        }
    }
}