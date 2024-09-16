using StoreHub.Core.SharedRepository;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Services.Interfaces
{
    public interface ICartService
    {
        public Task<CartToReturnDTO?> GetCartByUserIdAsync(string userId);
        public Task<bool> AddItemToCartAsync(string userId, CartItemDTO cartItemDTO);
        public Task<bool> DeleteCartItemFromCart(string userId, int cartItemId);
        decimal CalculateTotalPrice(IEnumerable<CartItem> cartItems);
        //public Task<bool> CheckoutAsync(string userId);
        //public Task<string> CreatePaymentIntent(decimal amount);
        //public Task<Order> CreateOrderFromCart(Cart cart, string userId);
    }
}
