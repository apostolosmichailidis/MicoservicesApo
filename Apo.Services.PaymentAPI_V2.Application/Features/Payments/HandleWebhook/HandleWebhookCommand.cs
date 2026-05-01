using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.HandleWebhook
{
    public record HandleWebhookCommand(string PaymentIntentId, string EventType) : IRequest<bool>;
}
