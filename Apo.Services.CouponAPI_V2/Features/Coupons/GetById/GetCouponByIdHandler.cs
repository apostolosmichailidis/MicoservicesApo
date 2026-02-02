using Apo.Services.CouponAPI_V2.Infrastructure;
using AutoMapper;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Features.Coupons.GetById
{
    public class GetCouponByIdHandler : IRequestHandler<GetCouponByIdQuery, CouponDto>
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public GetCouponByIdHandler(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CouponDto> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _repo.GetByIdAsync(request.Id);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
