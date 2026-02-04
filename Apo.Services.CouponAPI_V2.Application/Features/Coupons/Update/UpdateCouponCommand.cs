using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Update
{
    public record UpdateCouponCommand(CouponDto Coupon) : IRequest<CouponDto>;
}
