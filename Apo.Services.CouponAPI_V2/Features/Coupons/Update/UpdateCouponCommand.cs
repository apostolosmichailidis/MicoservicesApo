using MediatR;

namespace Apo.Services.CouponAPI_V2.Features.Coupons.Update
{
    public record UpdateCouponCommand(CouponDto Coupon) : IRequest<CouponDto>;
}
