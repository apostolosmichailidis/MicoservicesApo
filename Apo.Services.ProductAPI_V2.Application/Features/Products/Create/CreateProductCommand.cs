using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Create
{
    public record CreateProductCommand(ProductDto Product) : IRequest<ProductDto>;
}
