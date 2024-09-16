using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.Data.DbContext;
using StoreHub.Models.Entities;
using StoreHub.UnitOfWork;

namespace StoreHub.Core.Repository.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly StoreHubContext _context;

        public CartRepository(StoreHubContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByuserIdAsync(string userId)
            => await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        
    }
}
