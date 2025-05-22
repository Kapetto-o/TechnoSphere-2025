using System.ComponentModel;

namespace TechnoSphere_2025.models
{
    public class ComparisonRow : INotifyPropertyChanged
    {
        public string DisplayName { get; }

        private readonly Dictionary<int, string> _valuesByProduct;

        public ComparisonRow(string displayName, Dictionary<int, string> valuesByProduct)
        {
            DisplayName = displayName;
            _valuesByProduct = valuesByProduct;
        }

        public string this[int productId]
        {
            get
            {
                if (_valuesByProduct.TryGetValue(productId, out var val))
                    return val;
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}