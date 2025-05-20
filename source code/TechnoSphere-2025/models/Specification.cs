namespace TechnoSphere_2025.models
{
    public class SpecificationType
    {
        public int SpecTypeID { get; set; }
        public int CategoryID { get; set; }
        public string Name_Ru { get; set; } = "";
        public string Name_Eng { get; set; } = "";
        public bool IsMain { get; set; }
    }

    public class ProductSpecification
    {
        public int ProductID { get; set; }
        public int SpecTypeID { get; set; }
        public string Value_Ru { get; set; } = "";
        public string Value_Eng { get; set; } = "";
    }
}
