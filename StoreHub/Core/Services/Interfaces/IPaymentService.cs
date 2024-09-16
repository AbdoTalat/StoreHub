using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<List<Payment>> GetPaymentByUserId(string userId);
        public Task<string> CreatePaymentIntent(decimal amount);

    }
}
