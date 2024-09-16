using AutoMapper;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Mapping
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<Cart, CartToReturnDTO>()
                .ForMember(dest => dest.cartItemstoReturnDTOs, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemstoReturnDTO>()
                .ForMember(dest => dest.ProductsDataCart, opt => opt.MapFrom(src => src.Product));

            CreateMap<Product, ProductsData>();



            CreateMap<CartItemDTO, CartItem>();
        }
    }
}
    