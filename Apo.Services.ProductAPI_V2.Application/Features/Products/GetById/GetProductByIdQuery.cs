using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.GetById
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;
}
