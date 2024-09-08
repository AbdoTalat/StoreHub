using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }


        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
