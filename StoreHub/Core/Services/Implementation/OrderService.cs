using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

using StoreHub.UnitOfWork;
using Stripe;

namespace StoreHub.Core.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ICartRepository cartRepository, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cartRepository = cartRepository;
            _paymentService = paymentService;
        }

        public async Task<IEnumerable<OrdersToReturnDTO>?> GetOrdersbyUserIdAsync(string userId)
        {
            var query = _unitOfWork.Repository<Order>().GetAllAsQueryable();
            var orders = await query.Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId).ToListAsync();

            if (orders == null)
                return null;

            return _mapper.Map<IEnumerable<OrdersToReturnDTO>>(orders);
        }

        public async Task<bool> DeleteOrderAsync(int Id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(Id);
            if (order == null)
                return false;
            if (!await isOrderOwnedByUser(Id, order.UserId))
                return false;

            return await _unitOfWork.Repository<Order>().DeleteAsync(Id);
        }

        public async Task<bool> isOrderOwnedByUser(int orderId, string userId)
        {
            var order = await _unitOfWork.Repository<Order>().GetAllAsQueryable(o => o.ID == orderId).FirstOrDefaultAsync();

            if (order != null && order.UserId == userId)
                return true;

            return false;
        }


        public async Task<bool> CheckoutAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByuserIdAsync(userId);

            if (cart == null)
                throw new Exception("Cart not found for the specified user.");


            var paymentIntentId = await _paymentService.CreatePaymentIntent(cart.TotalPrice);

            if (string.IsNullOrEmpty(paymentIntentId))
                return false;

            var payment = new Payment
            {
                PaymentIntentID = paymentIntentId,
                Amount = cart.TotalPrice,
                Currency = "usd",
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Pending,
                UserId = userId,
            };
            await _unitOfWork.Repository<Payment>().AddNewAsync(payment);

            var order = await CreateOrderFromCart(cart, userId);
            order.PaymentId = payment.ID;

            await _unitOfWork.Repository<Order>().AddNewAsync(order);

            await _unitOfWork.Repository<Cart>().DeleteAsync(cart.ID);

            return true;
        }

        public async Task<Order> CreateOrderFromCart(Cart cart, string userId)
        {
            var address = await _unitOfWork.Repository<StoreHub.Models.Entities.Address>().GetAllAsQueryable().FirstOrDefaultAsync(a => a.UserId == userId);

            if (address == null)
                throw new Exception("The User need to provide an address first to make an Order!");
            var order = new Order
            {
                UserId = cart.UserId,
                AddressId = address.ID,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.TotalPrice,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    TotalPrice = ci.SubTotal
                }).ToList()
            };

            return order;
        }
    }
}
