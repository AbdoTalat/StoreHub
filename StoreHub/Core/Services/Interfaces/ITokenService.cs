using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface ITokenService
    {
        public DateTime GetTokenExpirationTime();
        public Task<string> GenerateJwtTokenAsync(ApplicationUser user, IConfiguration configuration);
    }
}
