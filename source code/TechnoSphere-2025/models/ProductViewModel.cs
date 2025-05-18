using System.ComponentModel;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly Product _model;


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
        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite == value) return;
                _isFavorite = value;
                OnProp(nameof(IsFavorite));
            }
        }

        public ICommand ToggleFavoriteCommand { get; }

        public ProductViewModel(Product p)
        {
            _model = p;
            _isFavorite = FavoritesRepository.IsFavorite(SessionManager.CurrentUserID, p.ProductID);

            ToggleFavoriteCommand = new DelegateCommand<object>(async _ =>
            {
                if (IsFavorite)
                    await FavoritesRepository.RemoveFavorite(SessionManager.CurrentUserID, ProductID);
                else
                    await FavoritesRepository.AddFavorite(SessionManager.CurrentUserID, ProductID);

                IsFavorite = !IsFavorite;
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnProp(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
