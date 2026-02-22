using MediatR;

namespace Apo.Services.ProductAPI_V2.Application.Features.Products.Delete
{
    public class DeleteProductHandler
        : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repo;

        public DeleteProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id);
        }
    }

}
