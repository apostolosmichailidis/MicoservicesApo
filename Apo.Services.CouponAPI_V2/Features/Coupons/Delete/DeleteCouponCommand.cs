using MediatR;

namespace Apo.Services.CouponAPI_V2.Features.Coupons.Delete
{
    public record DeleteCouponCommand(int Id) : IRequest<bool>;
}
