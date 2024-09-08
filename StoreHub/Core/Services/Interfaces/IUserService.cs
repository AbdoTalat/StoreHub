using Microsoft.AspNetCore.Identity;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO user);
        public Task<ApplicationUser?> GetUserByNameAsync(string userName);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        public Task<UserToReturnDTO> GetUserByIdAsync(string userId);

        public Task<AddressDTO> GetUserAddressAsync(string userId);
        public Task<bool> AddOrUpdateAddressAsync(AddressDTO addressDTO, string userId);


    }
}
