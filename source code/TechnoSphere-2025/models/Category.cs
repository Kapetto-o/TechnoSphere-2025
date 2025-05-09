namespace TechnoSphere_2025.models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string Name_Ru { get; set; } = string.Empty;
        public string Name_Eng { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
