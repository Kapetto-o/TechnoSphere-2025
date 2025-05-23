using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class ProductViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Product _model;
        private bool _isFavorite;
        private bool _isInBasket;
        private bool _isInComparison;

        public int CategoryID => _model.CategoryID;
        public int ProductID => _model.ProductID;

        public string Name => LocalizationManager.CurrentLanguage == LanguageType.Russian
                                  ? _model.Name_Ru
                                  : _model.Name_Eng;

        public string? ImagePath => _model.MainImagePath;

        // Базовая цена
        public decimal Price
        {
            get => _model.Price;
            private set
            {
                if (_model.Price != value)
                {
                    _model.Price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        // Акционная цена (nullable)
        public decimal? PromoPrice
        {
            get => _model.PromoPrice;
            private set
            {
                if (_model.PromoPrice != value)
                {
                    _model.PromoPrice = value;
                    OnPropertyChanged(nameof(PromoPrice));
                }
            }
        }

        public decimal? Installment => _model.InstallmentPrice;
        public bool InStock => _model.StockQuantity > 0;

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite == value) return;
                _isFavorite = value;
                OnPropertyChanged(nameof(IsFavorite));
            }
        }

        public bool IsInBasket
        {
            get => _isInBasket;
            set
            {
                if (_isInBasket == value) return;
                _isInBasket = value;
                OnPropertyChanged(nameof(IsInBasket));
            }
        }

        public bool IsInComparison
        {
            get => _isInComparison;
            set
            {
                if (_isInComparison == value) return;
                _isInComparison = value;
                OnPropertyChanged(nameof(IsInComparison));
            }
        }

        public ObservableCollection<CharacteristicViewModel> MainCharacteristics { get; }
            = new ObservableCollection<CharacteristicViewModel>();

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand AddToBasketCommand { get; }

        public ProductViewModel(Product model)
        {
            _model = model;

            int userId = SessionManager.CurrentUserID;
            _isInComparison = ComparisonRepository.IsInComparison(userId, ProductID);
            _isInBasket = BasketRepository.IsInBasket(userId, ProductID);
            _isFavorite = FavoritesRepository.IsFavorite(userId, ProductID);

            ToggleFavoriteCommand = new DelegateCommand<object>(async _ =>
            {
                if (IsFavorite)
                    await FavoritesRepository.RemoveFavorite(userId, ProductID);
                else
                    await FavoritesRepository.AddFavorite(userId, ProductID);
            });

            AddToBasketCommand = new DelegateCommand<object>(_ =>
            {
                try
                {
                    BasketRepository.AddToBasket(userId, ProductID);
                    IsInBasket = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось добавить товар в корзину:\n{ex.Message}",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            });

            LoadMainCharacteristics();

            FavoritesRepository.FavoriteChanged += OnGlobalFavoriteChanged;
            ComparisonRepository.ComparisonChanged += OnGlobalComparisonChanged;
        }

        /// <summary>
        /// Обновляет сразу базовую цену и акционную.
        /// </summary>
        public void SetPrices(decimal newPrice, decimal? newPromoPrice)
        {
            Price = newPrice;
            PromoPrice = newPromoPrice;
        }

        private void OnGlobalFavoriteChanged(object? sender, FavoriteChangedEventArgs e)
        {
            if (e.UserID != SessionManager.CurrentUserID) return;
            if (e.ProductID != ProductID) return;
            Application.Current.Dispatcher.Invoke(() => IsFavorite = e.IsAdded);
        }

        private void OnGlobalComparisonChanged(object? sender, ComparisonChangedEventArgs e)
        {
            if (e.UserID != SessionManager.CurrentUserID) return;
            if (e.ProductID != ProductID) return;
            Application.Current.Dispatcher.Invoke(() => IsInComparison = e.IsAdded);
        }

        private void LoadMainCharacteristics()
        {
            var specTypes = SpecificationRepository.GetTypesByCategory(_model.CategoryID);
            var specValues = SpecificationRepository
                                .GetSpecificationsForProduct(_model.ProductID)
                                .ToDictionary(v => v.SpecTypeID);

            var mainTypes = specTypes.Where(t => t.IsMain).Take(5);
            foreach (var type in mainTypes)
            {
                if (!specValues.TryGetValue(type.SpecTypeID, out var valueModel))
                    continue;

                var displayName = LocalizationManager.CurrentLanguage == LanguageType.Russian
                    ? type.Name_Ru
                    : type.Name_Eng;
                var displayValue = LocalizationManager.CurrentLanguage == LanguageType.Russian
                    ? valueModel.Value_Ru
                    : valueModel.Value_Eng;

                MainCharacteristics.Add(
                    new CharacteristicViewModel(displayName, displayValue));
            }
        }

        public void Dispose()
        {
            FavoritesRepository.FavoriteChanged -= OnGlobalFavoriteChanged;
            ComparisonRepository.ComparisonChanged -= OnGlobalComparisonChanged;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}