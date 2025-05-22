using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class SpecificationType
    {
        public int SpecTypeID { get; set; }
        public int CategoryID { get; set; }
        public string Name_Ru { get; set; } = "";
        public string Name_Eng { get; set; } = "";
        public int SortOrder { get; set; }
        public bool IsMain { get; set; }

        public string DisplayName
        {
            get => LocalizationManager.CurrentLanguage == LanguageType.Russian ? Name_Ru : Name_Eng;
        }
    }

    public class ProductSpecification
    {
        public int ProductID { get; set; }
        public int SpecTypeID { get; set; }
        public string Value_Ru { get; set; } = "";
        public string Value_Eng { get; set; } = "";
    }
}
