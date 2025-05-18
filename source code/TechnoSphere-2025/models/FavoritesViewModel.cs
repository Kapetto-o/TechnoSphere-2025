using System.Collections.ObjectModel;
using System.ComponentModel;
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
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
