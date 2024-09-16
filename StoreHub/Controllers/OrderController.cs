using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Helpers;
using StoreHub.Models.DTOs;
using StoreHub.Validations;
using System.Security.Claims;

namespace StoreHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpGet("Get-User-Orders")]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiErrorResponse(401));

            var orders = await orderService.GetOrdersbyUserIdAsync(userId);
            if(orders == null) 
                return NotFound(new ApiErrorResponse(404));

            return Ok(orders);
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized(new ApiErrorResponse(401));

                bool isSuccess = await orderService.CheckoutAsync(userId);

                if (!isSuccess)
                    return BadRequest("CheckOut Failed");

                return Ok("CheckOut Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("Delete-Order-By-ID{OrderId:int}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int OrderId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized(new ApiErrorResponse(401));

                bool isDeleted = await orderService.DeleteOrderAsync(OrderId);
                if (!isDeleted)
                    return NotFound(new ApiErrorResponse(404));

                return Ok($"Order With Id: {OrderId} Deleted Successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}