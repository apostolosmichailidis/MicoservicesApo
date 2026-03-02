using AutoMapper;
using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetByOrderId
{
    public class GetPaymentsByOrderIdHandler : IRequestHandler<GetPaymentsByOrderIdQuery, IEnumerable<PaymentDto>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IMapper _mapper;

        public GetPaymentsByOrderIdHandler(IPaymentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> Handle(GetPaymentsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var records = await _repo.GetByOrderIdAsync(request.OrderId);
            return _mapper.Map<IEnumerable<PaymentDto>>(records);
        }
    }
}
