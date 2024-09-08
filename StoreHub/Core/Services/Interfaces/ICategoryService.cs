using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<Category?> GetCategoryByIdAsync(int Id);
        public Task<CategoryToReturnDTO?> GetCategoryByIdWithProductsAsync(int Id);
        public Task<IEnumerable<CategoryToReturnDTO>> GetAllCategoriesWithProductsAsync();
        public Task<Category?> AddNewCategoryAsync(CategoryDTO categoryDTO);
        public Task<bool> UpdateCategoryAsync(int Id, CategoryDTO categoryDTO);
        public Task<bool> DeleteCategoryAsync(int Id);
    }
}
