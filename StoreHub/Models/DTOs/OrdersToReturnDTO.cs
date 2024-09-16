using StoreHub.Models.Entities;

namespace StoreHub.Models.DTOs
{
    public class OrdersToReturnDTO
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<orderItemsDTO> orderItemsDTOs { get; set; } = Enumerable.Empty<orderItemsDTO>();
    }

    public class orderItemsDTO
    {
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductsData productDataDTO { get; set; }
    }
}
