using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;
using StoreHub.UnitOfWork;
using Stripe;
using System.Configuration;

public class StripeConfiguration11
{
    public static void Configure()
    {
        StripeConfiguration.ApiKey  = "sk_test_51PvgCbRpeHss5rPypZMOBaRdyxNSkYwWSsCBqcD4kx8tCQoLDksqj6aFhanPJfPfAx1duu4efWg5l6eujXb7yvml00O04Ai68l";
    }
}
public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper, ICartRepository cartRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cartRepository = cartRepository;
        StripeConfiguration11.Configure();
    }

    public async Task<CartToReturnDTO?> GetCartByUserIdAsync(string userId)
    {
        var cartToReturn = await _unitOfWork.Repository<Cart>().GetSingleWithIncludeAsync(c => c.UserId == userId,
            query => query
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product));

        return _mapper.Map<CartToReturnDTO>(cartToReturn);
    }

    public async Task<bool> AddItemToCartAsync(string userId, CartItemDTO cartItemDTO)
    {
        var cart = await _cartRepository.GetCartByuserIdAsync(userId);

        var cartItem = _mapper.Map<CartItem>(cartItemDTO);

        var product = await _unitOfWork.Repository<StoreHub.Models.Entities.Product>().GetByIdAsync(cartItem.ProductId);

        if (product == null)
        {
            throw new Exception($"Product with ID {cartItem.ProductId} not found.");
        }

        cartItem.Product = product;

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CartDate = DateTime.UtcNow,
                TotalPrice = 0 
            };

            cartItem.CartId = cart.ID; 
            cartItem.SubTotal = cartItem.Product.Price * cartItem.Quantity;
            cart.CartItems.Add(cartItem);

            await _unitOfWork.Repository<Cart>().AddNewAsync(cart);

            cart = await _cartRepository.GetCartByuserIdAsync(userId);
        }
        else
        {
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                existingCartItem.SubTotal = existingCartItem.Product.Price * existingCartItem.Quantity;
            }
            else
            {
                cartItem.CartId = cart.ID;
                cartItem.SubTotal = cartItem.Product.Price * cartItem.Quantity;
                cart.CartItems.Add(cartItem);
            }
        }

        cart.TotalPrice = CalculateTotalPrice(cart.CartItems);

        bool isUpdated = await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

        return isUpdated;
    }

    public async Task<bool> DeleteCartItemFromCart(string userId, int cartItemId)
    {
        var cart = await _cartRepository.GetCartByuserIdAsync(userId);

        if (cart == null)
            throw new Exception("Cart not Found For specified User ID");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ID == cartItemId);

        if (cartItem == null)
            throw new Exception("Cart Item not Found!");

        cart.CartItems.Remove(cartItem);
        cart.TotalPrice = CalculateTotalPrice(cart.CartItems);

        bool isupdated = await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

        return isupdated;
    }

    public decimal CalculateTotalPrice(IEnumerable<CartItem> cartItems)
        => cartItems.Sum(ci => ci.SubTotal);

}


























//public async Task AddToCartAsync(string userId, int productId, int quantity)
//{
//    var cart = await GetCartByUserIdAsync(userId);
//    var product = await _context.Products.FindAsync(productId);

//    if (product == null || product.StockQuantity < quantity)
//        throw new Exception("Product unavailable or insufficient stock.");

//    if (cart == null)
//    {
//        cart = new Cart { UserId = userId };
//        _context.Carts.Add(cart);
//    }

//    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
//    if (cartItem == null)
//    {
//        cartItem = new CartItem { ProductId = productId, Quantity = quantity, CartId = cart.ID };
//        cart.CartItems.Add(cartItem);
//    }
//    else
//    {
//        cartItem.Quantity += quantity;
//    }

//    cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
//    await _context.SaveChangesAsync();
//}

//public async Task RemoveFromCartAsync(string userId, int productId)
//{
//    var cart = await GetCartByUserIdAsync(userId);
//    if (cart == null) return;

//    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
//    if (cartItem != null)
//    {
//        cart.CartItems.Remove(cartItem);
//        cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
//        await _context.SaveChangesAsync();
//    }
//}

//public async Task UpdateCartAsync(string userId, int productId, int quantity)
//{
//    var cart = await GetCartByUserIdAsync(userId);
//    if (cart == null) return;

//    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
//    if (cartItem != null)
//    {
//        cartItem.Quantity = quantity;
//        cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
//        await _context.SaveChangesAsync();
//    }
//}

//public async Task ClearCartAsync(string userId)
//{
//    var cart = await GetCartByUserIdAsync(userId);
//    if (cart != null)
//    {
//        _context.CartItems.RemoveRange(cart.CartItems);
//        cart.TotalPrice = 0;
//        await _context.SaveChangesAsync();
//    }
//}
//}
