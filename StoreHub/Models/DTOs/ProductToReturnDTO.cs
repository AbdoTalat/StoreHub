namespace StoreHub.Models.DTOs
{
    public class ProductToReturnDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }
    }
}
