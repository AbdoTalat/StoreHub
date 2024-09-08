using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities
{
    public class Payment
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public UserPaymentMethod PaymentMethod { get; set; }

        public string? ExternalTransactionId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public enum UserPaymentMethod
    {
        card,
        PayPal
    }
}
