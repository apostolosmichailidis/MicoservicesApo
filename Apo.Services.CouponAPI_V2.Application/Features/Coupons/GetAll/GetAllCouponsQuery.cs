using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetAll
{
    public record GetAllCouponsQuery : IRequest<IEnumerable<CouponDto>>;
}
