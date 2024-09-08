
namespace StoreHub.Models.DTOs
{
    public class CategoryToReturnDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductsData> productsDatas { get; set; } = Enumerable.Empty<ProductsData>();
    }
    public class ProductsData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
