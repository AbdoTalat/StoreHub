using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Models.DTOs;
using StoreHub.Models.Entities;

using StoreHub.UnitOfWork;

namespace StoreHub.Core.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<OrdersToReturnDTO?> AddNewOrderAsync(OrderToInsertDTO orderDTO, string userId)
        {
            var address = await _unitOfWork.Repository<Address>().GetAllAsQueryable().FirstOrDefaultAsync(a => a.UserId == userId);

            if (address == null)
                throw new Exception("The User need to provide an address first to make an Order!");

            var order = _mapper.Map<Order>(orderDTO);

            order.UserId = userId;
            order.AddressId = address.ID;
            order.TotalPrice = 0;
            order.OrderItems = new List<OrderItem>();

            foreach (var item in orderDTO.AddOrderItemsDTO)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                var orderItem = _mapper.Map<OrderItem>(item);
                orderItem.TotalPrice = item.Quantity * product.Price;
                orderItem.OrderId = order.ID;

                order.TotalPrice += orderItem.TotalPrice;
                order.OrderItems.Add(orderItem);
            }
            var addedOrder = await _unitOfWork.Repository<Order>().AddNewAsync(order);

            return _mapper.Map<OrdersToReturnDTO>(addedOrder);
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
    }
}
