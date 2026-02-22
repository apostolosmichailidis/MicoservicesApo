using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Delete
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
