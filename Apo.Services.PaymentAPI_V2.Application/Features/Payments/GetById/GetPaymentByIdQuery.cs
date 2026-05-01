using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetById
{
    public record GetPaymentByIdQuery(int Id) : IRequest<PaymentDto>;
}
