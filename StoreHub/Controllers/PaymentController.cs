using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using System.Security.Claims;

namespace StoreHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService1 _paymentService;

        public PaymentController(IPaymentService1 paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment-intent{orderId:int}/{paymentMethodId:alpha}")]
        public async Task<IActionResult> CreatePaymentIntent([FromRoute]int orderId,[FromRoute] string paymentMethodId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var clientSecret = await _paymentService.CreatePaymentIntentAsync(orderId, userId, paymentMethodId);
                return Ok(new { ClientSecret = clientSecret });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("handle-payment-success")]
        public async Task<IActionResult> HandlePaymentSuccess(string paymentIntentId, int paymentId)
        {
            try
            {
                await _paymentService.HandlePaymentSuccessAsync(paymentIntentId, paymentId);
                return Ok(new { Message = "Payment processed successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
