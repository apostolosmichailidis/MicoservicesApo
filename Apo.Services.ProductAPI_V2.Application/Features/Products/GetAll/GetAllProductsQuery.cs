using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.GetAll
{
    public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
}
