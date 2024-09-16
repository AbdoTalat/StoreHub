using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Data.DbContext;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;
using StoreHub.UnitOfWork;

namespace StoreHub.Core.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            mapper = _mapper;
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => _unitOfWork.Repository<Category>().GetAllAsEnumerableAsync();

        public Task<Category?> GetCategoryByIdAsync(int Id)
            => _unitOfWork.Repository<Category>().GetByIdAsync(Id);
        public async Task<CategoryToReturnDTO?> GetCategoryByIdWithProductsAsync(int Id)
        {
            var category = await _unitOfWork.Repository<Category>().GetSingleWithIncludeAsync(c => c.ID == Id, query => query.Include(c =>c.Products));
            if (category == null)
                return null;

            return mapper.Map<CategoryToReturnDTO>(category);
        }

        public async Task<IEnumerable<CategoryToReturnDTO>> GetAllCategoriesWithProductsAsync()
        {
            var category = await _unitOfWork.Repository<Category>().GetAllWithIncludeAsync(c => c.Products);

            return mapper.Map<IEnumerable<CategoryToReturnDTO>>(category);
        }

        public async Task<Category?> AddNewCategoryAsync(CategoryDTO CategoryDTO)
        {
            var catgeory = await _unitOfWork.Repository<Category>().AddNewAsync(mapper.Map<Category>(CategoryDTO));
            if (catgeory == null)
                return null;

            return catgeory;
        }

        public async Task<bool> UpdateCategoryAsync(int Id, CategoryDTO CategoryDTO)
        {
            var OldCategory = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (OldCategory == null)
                return false;

            mapper.Map(CategoryDTO, OldCategory);
            return await _unitOfWork.Repository<Category>().UpdateAsync(OldCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int Id)
            => await _unitOfWork.Repository<Category>().DeleteAsync(Id);
    }
}
