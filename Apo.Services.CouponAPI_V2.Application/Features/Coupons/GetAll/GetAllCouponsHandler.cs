using Apo.Services.CouponAPI_V2.Infrastructure;
using AutoMapper;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetAll
{
    public class GetAllCouponsHandler : IRequestHandler<GetAllCouponsQuery, IEnumerable<CouponDto>>
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public GetAllCouponsHandler(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CouponDto>> Handle(
            GetAllCouponsQuery request,
            CancellationToken cancellationToken)
        {
            var coupons = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
    }

}
