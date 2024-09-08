using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;
using StoreHub.UnitOfWork;

namespace StoreHub.Core.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IImageService _imageService, IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            imageService = _imageService;
            mapper = _mapper;
        }

        public async Task<IEnumerable<ProductToReturnDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllWithIncludeAsync(p => p.Category);

            return mapper.Map<IEnumerable<ProductToReturnDTO>>(products);
        }

        public async Task<ProductToReturnDTO?> GetProductByIdAsync(int? Id)
        {
            var product = await _unitOfWork.Repository<Product>().GetSingleWihtIncludeAsync(p => p.ID == Id, p => p.Category);

            return mapper.Map<ProductToReturnDTO>(product);
        }

        public async Task<Product?> AddNewProductWithImageAsync(ProductDTO productDTO)
        {
            string? imageUrl = productDTO.Image != null ? await imageService.SaveImageAsync(productDTO.Image) : null;

            var product = mapper.Map<Product>(productDTO);
            product.ImageUrl = imageUrl;

            return await _unitOfWork.Repository<Product>().AddNewAsync(product);
        }

        public async Task<bool> UpdateProductAsync(int Id, ProductDTO productDTO)
        {
            var OldProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (OldProduct == null)
                return false;

            string? imageUrl = productDTO.Image != null ? await imageService.SaveImageAsync(productDTO.Image) : null;

            mapper.Map(productDTO, OldProduct);
            OldProduct.ImageUrl = imageUrl;

            return await _unitOfWork.Repository<Product>().UpdateAsync(OldProduct);
        }

        public async Task<bool> DeleteProductWithImageAsync(int Id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (product == null)
                return false;

            if (product.ImageUrl != null)
                await imageService.DeleteImageAsync(product.ImageUrl);

            return await _unitOfWork.Repository<Product>().DeleteAsync(Id);
        }

        public async Task<(int totalItems, IEnumerable<ProductToReturnDTO> products)> SearchProductsWithPaging(
            int pageNumber,
            int pageSize,
            string categoryName,
            string productname)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _unitOfWork.Repository<Product>().GetAllWithIncludeAsQueryable(null, p => p.Category);

            if (!string.IsNullOrEmpty(productname))
            {
                query = query.Where(c => c.Name.Contains(productname));
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(c => c.Category.Name.Contains(categoryName));
            }

            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => mapper.Map<ProductToReturnDTO>(p))
                .ToListAsync();

            return (totalItems, products);
        }
    }
}
