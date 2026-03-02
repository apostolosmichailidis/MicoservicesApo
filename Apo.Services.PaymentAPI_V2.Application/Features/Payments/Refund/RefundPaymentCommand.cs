using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.Refund
{
    public record RefundPaymentCommand(int PaymentId) : IRequest<PaymentDto>;
}
