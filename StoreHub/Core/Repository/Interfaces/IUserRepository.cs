using Microsoft.AspNetCore.Identity;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<IdentityResult> CreateUserAsync(UserRegistrationDTO user);
        public Task<ApplicationUser?> GetUserByNameAsync(string userName);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        public Task<ApplicationUser?> GetUserByIdAsync(string userId);
    }
}
