using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class BasketViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<BasketItemViewModel> Items { get; }
            = new ObservableCollection<BasketItemViewModel>();

        public decimal TotalSum
        {
            get => Items.Sum(it => it.TotalPrice);
        }

        public ICommand ClearBasketCommand { get; }
        public ICommand CheckoutCommand { get; }

        public BasketViewModel()
        {
            int userId = SessionManager.CurrentUserID;
            var list = BasketRepository.GetBasketItems(userId);

            foreach (var dto in list)
            {
                var pvm = new ProductViewModel(dto.Product);
                var bivm = new BasketItemViewModel(pvm, dto.Quantity);

                bivm.QuantityChanged += OnItemQuantityChanged;
                bivm.ItemRemoved += OnItemRemoved;

                Items.Add(bivm);
            }

            ClearBasketCommand = new RelayCommand(_ =>
            {
                try
                {
                    BasketRepository.ClearBasket(userId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось очистить корзину:\n{ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var item in Items.ToList())
                {
                    item.QuantityChanged -= OnItemQuantityChanged;
                    item.ItemRemoved -= OnItemRemoved;
                    item.Dispose();
                }
                Items.Clear();
                OnPropertyChanged(nameof(TotalSum));
            }, _ => Items.Any());

            CheckoutCommand = new RelayCommand(_ =>
            {
                MessageBox.Show("Оформление заказа пока не реализовано.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }, _ => Items.Any());
        }

        private void OnItemQuantityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TotalSum));
        }

        private void OnItemRemoved(object? sender, EventArgs e)
        {
            if (sender is not BasketItemViewModel bivm) return;

            bivm.QuantityChanged -= OnItemQuantityChanged;
            bivm.ItemRemoved -= OnItemRemoved;
            bivm.Dispose();

            Items.Remove(bivm);

            OnPropertyChanged(nameof(TotalSum));

            (ClearBasketCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CheckoutCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        public void Dispose()
        {
            foreach (var item in Items)
            {
                item.QuantityChanged -= OnItemQuantityChanged;
                item.ItemRemoved -= OnItemRemoved;
                item.Dispose();
            }
        }
    }
}