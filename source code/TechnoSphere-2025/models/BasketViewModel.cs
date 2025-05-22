using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;
using TechnoSphere_2025.models;
using TechnoSphere_2025.modules.customer;

namespace TechnoSphere_2025.models
{
    public class BasketViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<BasketItemViewModel> Items { get; }
            = new ObservableCollection<BasketItemViewModel>();

        public decimal TotalSum => Items.Sum(it => it.TotalPrice);

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
                (CheckoutCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ClearBasketCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }, _ => Items.Any());

            CheckoutCommand = new RelayCommand(_ =>
            {
                var orderItems = Items.Select(vm => new BasketItemData
                {
                    Product = new Product
                    {
                        ProductID = vm.ProductVm.ProductID,
                        Name_Ru = vm.ProductVm.Name,
                        Name_Eng = vm.ProductVm.Name,
                        MainImagePath = vm.ProductVm.ImagePath,
                        Price = vm.ProductVm.Price,
                        InstallmentPrice = vm.ProductVm.Installment,
                        PromoPrice = vm.ProductVm.PromoPrice,
                        StockQuantity = vm.ProductVm.InStock ? 1 : 0,
                        IsActive = true,
                        CategoryID = vm.ProductVm.ProductID
                    },
                    Quantity = vm.Quantity
                }).ToList();

                var dlg = new Window_PlacingOrder
                {
                    Owner = Application.Current.MainWindow
                };
                bool? result = dlg.ShowDialog();
                if (result != true) return;

                try
                {
                    OrderRepository.CreateOrder(
                        SessionManager.CurrentUserID,
                        dlg.ContactName,
                        dlg.ContactPhone,
                        dlg.DeliveryAddress,
                        orderItems
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось создать заказ:\n{ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    BasketRepository.ClearBasket(SessionManager.CurrentUserID);
                }
                catch
                {
                }

                foreach (var item in Items.ToList())
                {
                    item.QuantityChanged -= OnItemQuantityChanged;
                    item.ItemRemoved -= OnItemRemoved;
                    item.Dispose();
                }
                Items.Clear();
                OnPropertyChanged(nameof(TotalSum));

                (CheckoutCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ClearBasketCommand as RelayCommand)?.RaiseCanExecuteChanged();

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
            (CheckoutCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ClearBasketCommand as RelayCommand)?.RaiseCanExecuteChanged();
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