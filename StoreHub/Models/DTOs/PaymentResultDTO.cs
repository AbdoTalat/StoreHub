namespace StoreHub.Models.DTOs
{
    public class PaymentResultDTO
    {
        public bool Success { get; set; }
        public string ChargeId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
