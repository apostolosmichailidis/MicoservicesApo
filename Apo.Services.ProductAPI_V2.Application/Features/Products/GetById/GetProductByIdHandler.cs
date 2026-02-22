using AutoMapper;
using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.GetById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _repo.GetByIdAsync(request.Id);
            return _mapper.Map<ProductDto>(coupon);
        }
    }
}
