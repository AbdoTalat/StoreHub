using AutoMapper;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>();

            //CreateMap<Category, GetCategoryWithProductsDTO>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.productsDatas, opt => opt.MapFrom(src => src.Products));

            //CreateMap<Product, ProductsData>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            //    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));


            CreateMap<Category, CategoryToReturnDTO>()
                .ForMember(dest => dest.productsDatas, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, ProductsData>();

        }
    }
}
