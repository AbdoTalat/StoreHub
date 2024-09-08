using AutoMapper;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
