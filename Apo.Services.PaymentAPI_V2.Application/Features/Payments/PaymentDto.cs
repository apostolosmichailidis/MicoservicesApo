namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string Status { get; set; } = string.Empty;
        public string? StripePaymentIntentId { get; set; }
        public string? StripeClientSecret { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
