using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.UnitOfWork;

namespace StoreHub.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository _userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            userRepository = _userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO user)
            => await userRepository.CreateUserAsync(user);

        public async Task<ApplicationUser?> GetUserByNameAsync(string userName)
            => await userRepository.GetUserByNameAsync(userName);

        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
            => await userRepository.CheckUserPasswordAsync(user, password);

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
            => await userRepository.GetUserRolesAsync(user);

        public async Task<UserToReturnDTO> GetUserByIdAsync(string userId)
            => _mapper.Map<UserToReturnDTO>(await userRepository.GetUserByIdAsync(userId));

        public async Task<AddressDTO> GetUserAddressAsync(string userId)
        {
            var address = await _unitOfWork.Repository<Address>().GetAllAsQueryable(a => a.UserId == userId).FirstOrDefaultAsync();
            if (address == null)
                return null;

            var mappedAddress = _mapper.Map<AddressDTO>(address);
            return mappedAddress;
        }

        public async Task<bool> AddOrUpdateAddressAsync(AddressDTO addressDTO, string userId)
        {
            var OldAddress = await _unitOfWork.Repository<Address>().GetAllAsQueryable(a => a.UserId == userId).FirstOrDefaultAsync();

            var Address = _mapper.Map<Address>(addressDTO);
            Address.UserId = userId;

            if (OldAddress == null)
            {
                var AddedAddress = _unitOfWork.Repository<Address>().AddNewAsync(Address);

                return AddedAddress != null;
            }
            else
            {
                _mapper.Map(addressDTO, OldAddress);
                OldAddress.UserId = userId;

                return await _unitOfWork.Repository<Address>().UpdateAsync(OldAddress);
            }
        }
    }
}
