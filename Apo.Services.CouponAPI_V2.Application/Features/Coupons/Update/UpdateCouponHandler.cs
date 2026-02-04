using Apo.Services.CouponAPI_V2.Domain;
using Apo.Services.CouponAPI_V2.Infrastructure;
using AutoMapper;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Update
{
    public class UpdateCouponHandler : IRequestHandler<UpdateCouponCommand, CouponDto>
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public UpdateCouponHandler(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CouponDto> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var updated = await _repo.UpdateAsync(coupon);
            return _mapper.Map<CouponDto>(updated);
        }
    }

}
