using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class Payment
    {
        public int ID { get; set; }

        [Required]
        public string PaymentIntentID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }


        [DisplayFormat(DataFormatString = "0:yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus PaymentStatus { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}
