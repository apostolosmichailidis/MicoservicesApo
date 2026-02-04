using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Create
{
    public record CreateCouponCommand(CouponDto Coupon) : IRequest<CouponDto>;
}
