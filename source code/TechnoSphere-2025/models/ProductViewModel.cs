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

        public int ProductID => _model.ProductID;
        public string SKU => _model.SKU;
        public string Name => LocalizationManager.CurrentLanguage == LanguageType.Russian
                                      ? _model.Name_Ru
                                      : _model.Name_Eng;
        public string? ImagePath => _model.MainImagePath;
        public decimal Price => _model.Price;
        public decimal? PromoPrice => _model.PromoPrice;
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

        public ObservableCollection<CharacteristicViewModel> MainCharacteristics { get; }
            = new ObservableCollection<CharacteristicViewModel>();

        public ICommand ToggleFavoriteCommand { get; }

        public ProductViewModel(Product model)
        {
            _model = model;

            _isFavorite = FavoritesRepository.IsFavorite(
                SessionManager.CurrentUserID,
                ProductID);

            ToggleFavoriteCommand = new DelegateCommand<object>(async _ =>
            {
                if (IsFavorite)
                    await FavoritesRepository.RemoveFavorite(SessionManager.CurrentUserID, ProductID);
                else
                    await FavoritesRepository.AddFavorite(SessionManager.CurrentUserID, ProductID);
            });

            LoadMainCharacteristics();

            FavoritesRepository.FavoriteChanged += OnGlobalFavoriteChanged;
        }

        private void LoadMainCharacteristics()
        {
            var specTypes = SpecificationRepository
                .GetTypesByCategory(_model.CategoryID);

            var specValues = SpecificationRepository
                .GetSpecificationsForProduct(_model.ProductID)
                .ToDictionary(v => v.SpecTypeID);

            var mainTypes = specTypes
                .Where(t => t.IsMain)
                .Take(5);

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

        private void OnGlobalFavoriteChanged(object? sender, FavoriteChangedEventArgs e)
        {
            if (e.UserID != SessionManager.CurrentUserID) return;
            if (e.ProductID != ProductID) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsFavorite = e.IsAdded;
            });
        }

        public void Dispose()
        {
            FavoritesRepository.FavoriteChanged -= OnGlobalFavoriteChanged;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}