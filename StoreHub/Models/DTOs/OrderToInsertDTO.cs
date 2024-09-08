using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.DTOs
{
    public class OrderToInsertDTO
    {
        [Required]
        public List<OrderItemDTO> AddOrderItemsDTO { get; set; } = new List<OrderItemDTO>();
    }
    public class OrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}


