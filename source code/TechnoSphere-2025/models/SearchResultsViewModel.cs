using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class SearchResultsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductViewModel> Results { get; }
            = new ObservableCollection<ProductViewModel>();

        private string _query = "";
        public string Query
        {
            get => _query;
            set { _query = value; OnProp(nameof(Query)); }
        }

        public SearchResultsViewModel(string query)
        {
            Query = query;
            var all = ProductRepository.GetAllActiveProducts();
            var regex = new Regex(Regex.Escape(query), RegexOptions.IgnoreCase);

            foreach (var p in all)
            {
                var name = LocalizationManager.CurrentLanguage == LanguageType.Russian
                           ? p.Name_Ru
                           : p.Name_Eng;
                if (regex.IsMatch(name))
                    Results.Add(new ProductViewModel(p));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnProp(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
