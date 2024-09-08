using Stripe;
using StoreHub.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using StoreHub.UnitOfWork;
using StoreHub.Models.Entities;
using StoreHub.Data.DbContext;
using StoreHub.Core.Services.Interfaces;

namespace StoreHub.Core.Services.Implementation
{
    public class PaymentService1 : IPaymentService1
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly StoreHubContext _context;

        public PaymentService1(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork,
            IConfiguration configuration, StoreHubContext context)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _context = context;

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreatePaymentIntentAsync(int orderId, string userId, string paymentMethodId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ID == orderId && o.UserId == userId);

            if (order == null)
            {
                throw new ArgumentException("Order not found or does not belong to this user.");
            }

            var amount = (long)(order.OrderItems.Sum(item => item.Product.Price * item.Quantity) * 100); // Amount in cents

            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                Metadata = new Dictionary<string, string> { { "OrderId", orderId.ToString() } },
                PaymentMethodTypes = new List<string> { "card" },
                PaymentMethod = paymentMethodId
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            var payment = new Payment
            {
                Amount = paymentIntent.Amount / 100m, // Convert from cents to dollars
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = UserPaymentMethod.card,
                ExternalTransactionId = paymentIntent.Id,
                PaymentStatus = paymentIntent.Status,
                ErrorMessage = paymentIntent.Status == "failed" ? "Payment failed" : null
            };

            await _paymentRepository.AddPaymentAsync(payment);

            order.PaymentId = payment.ID;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            return paymentIntent.ClientSecret;
        }

        public async Task HandlePaymentSuccessAsync(string paymentIntentId, int paymentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                if (paymentIntent == null)
                {
                    throw new ArgumentException("PaymentIntent not found.");
                }

                if (!paymentIntent.Metadata.TryGetValue("OrderId", out var orderIdStr) || !int.TryParse(orderIdStr, out var orderId))
                {
                    throw new ArgumentException("Order ID is invalid or not found in PaymentIntent metadata.");
                }

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.ID == orderId);

                if (order == null)
                {
                    throw new ArgumentException("Order not found.");
                }

                if (paymentIntent.Status == "succeeded")
                {
                    order.orderStatus = OrderStatus.Recieve;
                    await _unitOfWork.Repository<Order>().UpdateAsync(order);

                    var payment = await _unitOfWork.Repository<Payment>().GetByIdAsync(paymentId);
                    if (payment != null)
                    {
                        payment.PaymentStatus = paymentIntent.Status;
                        await _unitOfWork.Repository<Payment>().UpdateAsync(payment);
                    }
                }
                else
                {
                    throw new Exception("Payment was not successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing payment: {ex.Message}");
                throw;
            }
        }
    }
}
