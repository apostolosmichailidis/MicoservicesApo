namespace Apo.Services.PaymentAPI_V2.Application
{
    public interface IStripeService
    {
        Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
            decimal amount, string currency, string orderId);

        Task RefundAsync(string paymentIntentId);

        (string PaymentIntentId, string EventType)? ParseWebhookEvent(
            string json, string signature);
    }
}
