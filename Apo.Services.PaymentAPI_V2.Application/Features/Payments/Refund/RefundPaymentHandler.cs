using Apo.Services.PaymentAPI_V2.Application.Common.Exceptions;
using Apo.Services.PaymentAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.Refund
{
    public class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand, PaymentDto>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStripeService _stripeService;
        private readonly IMapper _mapper;

        public RefundPaymentHandler(IPaymentRepository repo, IStripeService stripeService, IMapper mapper)
        {
            _repo = repo;
            _stripeService = stripeService;
            _mapper = mapper;
        }

        public async Task<PaymentDto> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var record = await _repo.GetByIdAsync(request.PaymentId);
            if (record is null) throw new NotFoundException($"Payment {request.PaymentId} not found");

            if (record.StripePaymentIntentId is null)
                throw new ValidationException("Payment has no associated Stripe PaymentIntent");

            await _stripeService.RefundAsync(record.StripePaymentIntentId);

            record.Status = PaymentStatus.Refunded;
            record.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(record);
            return _mapper.Map<PaymentDto>(updated);
        }
    }
}
