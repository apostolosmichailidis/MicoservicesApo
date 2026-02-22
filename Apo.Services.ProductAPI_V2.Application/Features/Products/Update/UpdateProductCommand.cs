using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Update
{
    public record UpdateProductCommand(ProductDto Product) : IRequest<ProductDto>;
}
