using System.Collections.ObjectModel;
using System.ComponentModel;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly Category _model;
        public int CategoryID => _model.CategoryID;

        public ObservableCollection<CategoryViewModel> Children { get; }
            = new ObservableCollection<CategoryViewModel>();

        public CategoryViewModel(Category model)
        {
            _model = model;
            LocalizationManager.LanguageChanged += (_, __) =>
                OnPropertyChanged(nameof(DisplayName));
        }

        public string DisplayName =>
            LocalizationManager.CurrentLanguage == LanguageType.Russian
                ? _model.Name_Ru
                : _model.Name_Eng;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
