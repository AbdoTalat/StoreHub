using StoreHub.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegistrationDTO user)
        {
            var userNameCheck = await userManager.FindByNameAsync(user.UserName);
            if (userNameCheck != null)
                throw new InvalidOperationException("This User Name Already Exist!");

            var existUser = await userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
                throw new InvalidOperationException("This user Email Already Exist!");

            var NewUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var result = await userManager.CreateAsync(NewUser, user.Password);
            if (!result.Succeeded)
                throw new Exception("Error Occurred While Creating User");

            return result;
        }

        public async Task<ApplicationUser?> GetUserByNameAsync(string userName)
            => await userManager.FindByNameAsync(userName);

        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
            => await userManager.CheckPasswordAsync(user, password);

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
            => await userManager.GetRolesAsync(user);

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
            => await userManager.FindByIdAsync(userId);

    }
}
