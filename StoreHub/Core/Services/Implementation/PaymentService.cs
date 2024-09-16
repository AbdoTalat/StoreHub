using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.Entities;
using StoreHub.UnitOfWork;
using Stripe;

namespace StoreHub.Core.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Payment>> GetPaymentByUserId(string userId)
        {
            return await _unitOfWork.Repository<Payment>().GetAllAsQueryable(p => p.UserId == userId).ToListAsync();
        }


        public async Task<string> CreatePaymentIntent(decimal amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Convert amount to cents
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.Id;
        }

    }
}
