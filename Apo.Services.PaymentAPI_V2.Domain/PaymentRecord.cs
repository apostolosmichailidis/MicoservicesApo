using System.ComponentModel.DataAnnotations;

namespace Apo.Services.PaymentAPI_V2.Domain
{
    public class PaymentRecord
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "usd";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? StripePaymentIntentId { get; set; }

        public string? StripeClientSecret { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
