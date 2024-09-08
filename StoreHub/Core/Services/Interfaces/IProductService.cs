using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductToReturnDTO>> GetAllProductsAsync();
        public Task<ProductToReturnDTO?> GetProductByIdAsync(int? Id);
        public Task<Product?> AddNewProductWithImageAsync(ProductDTO productDTO);
        public Task<bool> UpdateProductAsync(int Id, ProductDTO product);
        public Task<bool> DeleteProductWithImageAsync(int Id);
        public Task<(int totalItems, IEnumerable<ProductToReturnDTO> products)> SearchProductsWithPaging(int pageNumber, int pageSize, string categoryName, string productname);
    }
}
