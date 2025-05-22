using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductViewModel> Favorites { get; }
            = new ObservableCollection<ProductViewModel>();

        public FavoritesViewModel()
        {
            var list = FavoritesRepository.GetFavorites(SessionManager.CurrentUserID);
            foreach (var p in list)
                Favorites.Add(new ProductViewModel(p));
            FavoritesRepository.FavoriteChanged += OnFavoriteChanged;
        }

        private void OnFavoriteChanged(object? sender, FavoriteChangedEventArgs e)
        {
            if (e.UserID != SessionManager.CurrentUserID) return;
            if (!e.IsAdded)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                bool alreadyExists = Favorites
                    .Any(vm => vm.ProductID == e.ProductID);
                if (alreadyExists)
                    return;
                var prod = FavoritesRepository.GetFavorites(e.UserID)
                               .FirstOrDefault(p => p.ProductID == e.ProductID);
                if (prod != null)
                    Favorites.Add(new ProductViewModel(prod));
            });
        }

        public void Dispose()
        {
            FavoritesRepository.FavoriteChanged -= OnFavoriteChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
