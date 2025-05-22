using System.Collections.ObjectModel;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class ComparisonTabViewModel
    {
        public int CategoryID { get; }
        public string CategoryName { get; }
        public ObservableCollection<ProductViewModel> Products { get; } = new();
        public ObservableCollection<SpecificationType> SpecTypes { get; } = new();
        public ObservableCollection<ComparisonRow> Rows { get; } = new();
        public event EventHandler? NotifyRowsChanged;

        public ComparisonTabViewModel(int categoryId, string categoryName)
        {
            CategoryID = categoryId;
            CategoryName = categoryName;
        }

        public void RefreshRows()
        {
            Rows.Clear();
            foreach (var specType in SpecTypes)
            {
                var dict = new Dictionary<int, string>();
                foreach (var prodVm in Products)
                {
                    var specs = SpecificationRepository
                                   .GetSpecificationsForProduct(prodVm.ProductID)
                                   .FirstOrDefault(s => s.SpecTypeID == specType.SpecTypeID);
                    string val = specs == null
                        ? string.Empty
                        : (LocalizationManager.CurrentLanguage == LanguageType.Russian
                             ? specs.Value_Ru
                             : specs.Value_Eng);
                    dict[prodVm.ProductID] = val;
                }

                var displayName = LocalizationManager.CurrentLanguage == LanguageType.Russian
                    ? specType.Name_Ru
                    : specType.Name_Eng;

                Rows.Add(new ComparisonRow(displayName, dict));
            }

            NotifyRowsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}