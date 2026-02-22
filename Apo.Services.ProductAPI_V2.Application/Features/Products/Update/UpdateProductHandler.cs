using Apo.Services.ProductAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Product>(request.Product);
            var updated = await _repo.UpdateAsync(coupon);
            return _mapper.Map<ProductDto>(updated);
        }
    }

}
