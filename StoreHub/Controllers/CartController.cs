using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Helpers;
using StoreHub.Models.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("Get-User-Cart")]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new ApiErrorResponse(401));

        var cart = await _cartService.GetCartByUserIdAsync(userId);
        if (cart == null) 
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("Update-Cart")]
    public async Task<IActionResult> AddNewCart(CartItemDTO cartItemDTO)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiErrorResponse(401));

            bool isSuccess = await _cartService.AddItemToCartAsync(userId, cartItemDTO);
            if (!isSuccess)
                return BadRequest(new ApiErrorResponse(400));

            return Ok("Item Added to Cart Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("Delete-CartItem{cartItemId:int}")]
    public async Task<IActionResult> RemoveFromCart([FromRoute]int cartItemId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiErrorResponse(401));

            bool isDeleted = await _cartService.DeleteCartItemFromCart(userId, cartItemId);

            if (!isDeleted)
                return BadRequest(new ApiErrorResponse(400));
            return Ok("Item deleted successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
