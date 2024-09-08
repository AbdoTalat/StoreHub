using AutoMapper;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order, OrdersToReturnDTO>()
                .ForMember(dest => dest.orderItemsDTOs, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, orderItemsDTO>()
                .ForMember(dest => dest.productDataDTO, opt => opt.MapFrom(src => src.Product));

            CreateMap<Product, productDataDTO>();



            CreateMap<OrderToInsertDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());


            CreateMap<OrderItemDTO, OrderItem>()
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
        }
    }
}
