using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Helpers;
using StoreHub.Models.DTOs;
using StoreHub.Validations;
using System.Security.Claims;
//using StoreHub.Models.DTOs;

namespace StoreHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public AccountController(IUserService _userService, ITokenService _tokenService, IConfiguration _configuration)
        {
            userService = _userService;
            tokenService = _tokenService;
            configuration = _configuration;
        }

        [HttpPost("User-Register")]
        public async Task<IActionResult> Registration(UserRegistrationDTO user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetModelStateErrors();
                return BadRequest(new { Error = errors });
            }
            try
            {
                await userService.RegisterUserAsync(user);
                return StatusCode(StatusCodes.Status201Created, "User registered Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("User-Login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetModelStateErrors();
                return BadRequest(new { Errors = errors });
            }

            var user = await userService.GetUserByNameAsync(userDTO.UserName);
            if (user == null || !await userService.CheckUserPasswordAsync(user, userDTO.Password))
                return Unauthorized("Incorrect Password Or User Name!");
            

            var token = await tokenService.GenerateJwtTokenAsync(user, configuration);
            var expiration = tokenService.GetTokenExpirationTime();

            return Ok(new
            {
                Token = token,
                Expiration = expiration
            });
        }

        [Authorize]
        [HttpGet("Get-Current-User")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var user = await userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-User-Address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized(new ApiErrorResponse(401));

            var Address = await userService.GetUserAddressAsync(userId);
            return Ok(Address);
        }

        [Authorize]
        [HttpPut("Update-User-Address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDTO addressDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetModelStateErrors();
                    return BadRequest(new { Errors = errors });
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized(new ApiErrorResponse(401));

                bool isUpdated = await userService.AddOrUpdateAddressAsync(addressDTO, userId);
                if (!isUpdated)
                    return BadRequest();

                return Ok("Address Updated Successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}