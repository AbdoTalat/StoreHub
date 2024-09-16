using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class Cart
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime CartDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; } 


        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
