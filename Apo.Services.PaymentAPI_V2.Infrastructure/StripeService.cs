using Apo.Services.PaymentAPI_V2.Application;
using Apo.Services.PaymentAPI_V2.Application.Settings;
using Microsoft.Extensions.Options;
using Stripe;

namespace Apo.Services.PaymentAPI_V2.Infrastructure
{
    public class StripeService : IStripeService
    {
        private readonly StripeClient _stripeClient;
        private readonly StripeOptions _options;

        public StripeService(IOptions<StripeOptions> options)
        {
            _options = options.Value;
            _stripeClient = new StripeClient(_options.SecretKey);
        }

        public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
            decimal amount, string currency, string orderId)
        {
            var service = new PaymentIntentService(_stripeClient);
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                },
                Metadata = new Dictionary<string, string> { { "orderId", orderId } }
            };

            var intent = await service.CreateAsync(createOptions);
            return (intent.Id, intent.ClientSecret);
        }

        public async Task RefundAsync(string paymentIntentId)
        {
            var service = new RefundService(_stripeClient);
            var refundOptions = new RefundCreateOptions { PaymentIntent = paymentIntentId };
            await service.CreateAsync(refundOptions);
        }

        public (string PaymentIntentId, string EventType)? ParseWebhookEvent(string json, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signature, _options.WebhookSecret);

                if (stripeEvent.Data.Object is PaymentIntent intent)
                {
                    var eventType = stripeEvent.Type switch
                    {
                        Events.PaymentIntentSucceeded => "succeeded",
                        Events.PaymentIntentPaymentFailed => "failed",
                        _ => stripeEvent.Type
                    };
                    return (intent.Id, eventType);
                }

                return null;
            }
            catch (StripeException)
            {
                return null;
            }
        }
    }
}
