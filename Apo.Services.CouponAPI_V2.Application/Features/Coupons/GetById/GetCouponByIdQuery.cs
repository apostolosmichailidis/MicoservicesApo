using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetById
{
    public record GetCouponByIdQuery(int Id) : IRequest<CouponDto>;
}
