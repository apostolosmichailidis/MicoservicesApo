using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetByOrderId
{
    public record GetPaymentsByOrderIdQuery(string OrderId) : IRequest<IEnumerable<PaymentDto>>;
}
