using Apo.Services.CouponAPI_V2.Application.Common.Exceptions;
using Apo.Services.CouponAPI_V2.Infrastructure;
using AutoMapper;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetByCode
{
    public class GetCouponByCodeHandler : IRequestHandler<GetCouponByCodeQuery, CouponDto>
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public GetCouponByCodeHandler(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CouponDto> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _repo.GetByCodeAsync(request.Code);

            if (coupon is null) 
                throw new NotFoundException("Coupon not found");

            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
