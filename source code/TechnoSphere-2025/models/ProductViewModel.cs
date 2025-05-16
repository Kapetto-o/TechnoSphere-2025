using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly Product _model;

        public ProductViewModel(Product p) => _model = p;

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

        // TODO: здесь можно добавить Rating, Features, и т.п.

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnProp(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
