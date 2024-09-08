using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class Order
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Range(0.0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;


        public int AddressId { get; set; }
        public virtual Address? Address { get; set; }

        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending,
        Recieve,
        Failed
    }
}
