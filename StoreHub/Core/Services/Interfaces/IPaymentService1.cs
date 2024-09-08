namespace StoreHub.Core.Services.Interfaces
{
    public interface IPaymentService1
    {
        public Task<string> CreatePaymentIntentAsync(int orderId, string userId, string paymentMethodId);
        public Task HandlePaymentSuccessAsync(string paymentIntentId, int paymentId);
    }
}
