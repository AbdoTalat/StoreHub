using AutoMapper;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, UserToReturnDTO>();

            CreateMap<Address, AddressDTO>();

            CreateMap<AddressDTO, Address>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

        }
    }
}
