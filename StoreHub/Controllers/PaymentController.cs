using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Helpers;
using System.Security.Claims;

namespace StoreHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("Get-Payment-For-User")]
        public async Task<IActionResult> GetPaymentByUserId()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiErrorResponse(401));

            var payment = await _paymentService.GetPaymentByUserId(userId);

            if (payment == null) 
                return NotFound();

            return Ok(payment);
        }
    }
}
