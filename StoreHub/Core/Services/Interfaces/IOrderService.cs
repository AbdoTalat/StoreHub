using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrdersToReturnDTO>?> GetOrdersbyUserIdAsync(string userId);
        public Task<bool> DeleteOrderAsync(int Id);
        Task<bool> isOrderOwnedByUser(int orderId, string userId);
        public Task<bool> CheckoutAsync(string userId);
        public Task<Order> CreateOrderFromCart(Cart cart, string userId);
    }
}
