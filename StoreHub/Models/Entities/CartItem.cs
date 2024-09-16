using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class CartItem
    {
        public int ID { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SubTotal { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
