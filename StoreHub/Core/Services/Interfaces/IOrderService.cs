using StoreHub.Models.DTOs;

namespace StoreHub.Core.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrdersToReturnDTO>?> GetOrdersbyUserIdAsync(string userId);
        public Task<OrdersToReturnDTO?> AddNewOrderAsync(OrderToInsertDTO orderDTO, string userId);
        public Task<bool> DeleteOrderAsync(int Id);
        public Task<bool> isOrderOwnedByUser(int orderId, string userId);
    }
}
