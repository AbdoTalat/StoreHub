using StoreHub.Models.Entities;

namespace StoreHub.Core.Repository.Interfaces
{
    public interface ICartRepository
    {
        public Task<Cart?> GetCartByuserIdAsync(string userId);
    }
}
