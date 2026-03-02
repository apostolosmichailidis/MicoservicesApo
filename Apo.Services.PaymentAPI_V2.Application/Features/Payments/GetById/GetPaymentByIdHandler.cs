using Apo.Services.PaymentAPI_V2.Application.Common.Exceptions;
using AutoMapper;
using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetById
{
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
    {
        private readonly IPaymentRepository _repo;
        private readonly IMapper _mapper;

        public GetPaymentByIdHandler(IPaymentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var record = await _repo.GetByIdAsync(request.Id);
            if (record is null) throw new NotFoundException($"Payment {request.Id} not found");
            return _mapper.Map<PaymentDto>(record);
        }
    }
}
