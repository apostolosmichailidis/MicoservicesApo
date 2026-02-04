using Apo.Services.CouponAPI_V2.Infrastructure;
using MediatR;

namespace Apo.Services.CouponAPI_V2.Application.Features.Coupons.Delete
{
    public class DeleteCouponHandler
        : IRequestHandler<DeleteCouponCommand, bool>
    {
        private readonly ICouponRepository _repo;

        public DeleteCouponHandler(ICouponRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id);
        }
    }

}
