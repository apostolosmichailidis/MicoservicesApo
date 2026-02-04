using Apo.Services.CouponAPI_V2.Domain;
using Apo.Services.CouponAPI_V2.Infrastructure;
using AutoMapper;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Create
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponCommand, CouponDto>
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public CreateCouponHandler(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CouponDto> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var created = await _repo.CreateAsync(coupon);
            return _mapper.Map<CouponDto>(created);
        }
    }

}
