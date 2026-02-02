using MediatR;

namespace Apo.Services.CouponAPI_V2.Features.Coupons.GetAll
{
    public record GetAllCouponsQuery : IRequest<IEnumerable<CouponDto>>;
}
