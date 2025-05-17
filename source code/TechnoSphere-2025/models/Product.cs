namespace TechnoSphere_2025.models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string SKU { get; set; } = "";
        public string Name_Ru { get; set; } = "";
        public string Name_Eng { get; set; } = "";
        public string? MainImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal? InstallmentPrice { get; set; }
        public decimal? PromoPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryID { get; set; }
    }
}
