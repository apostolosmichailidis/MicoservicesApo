using Apo.Services.ProductAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.Product);
            var created = await _repo.CreateAsync(product);
            return _mapper.Map<ProductDto>(created);
        }
    }

}
