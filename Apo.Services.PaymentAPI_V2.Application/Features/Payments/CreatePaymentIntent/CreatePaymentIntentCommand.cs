using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.CreatePaymentIntent
{
    public record CreatePaymentIntentCommand(
        string OrderId,
        string UserId,
        decimal Amount,
        string Currency = "usd") : IRequest<PaymentDto>;
}
