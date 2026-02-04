using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Delete
{
    public record DeleteCouponCommand(int Id) : IRequest<bool>;
}
