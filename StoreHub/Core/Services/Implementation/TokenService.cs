using StoreHub.Core.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreHub.Core.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository userRepository;

        public TokenService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        public DateTime GetTokenExpirationTime()
        {
            return DateTime.Now.AddHours(1);
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user, IConfiguration configuration)
        {
            var Claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var role = await userRepository.GetUserRolesAsync(user);
            foreach (var itemRole in role)
            {
                Claims.Add(new Claim(ClaimTypes.Role, itemRole));
            }

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken Token = new JwtSecurityToken(

                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: Claims,
                expires: GetTokenExpirationTime(),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
