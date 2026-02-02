using MediatR;

namespace Apo.Services.CouponAPI_V2.Features.Coupons.GetByCode
{
    public record GetCouponByCodeQuery(string Code) : IRequest<CouponDto>;
}
