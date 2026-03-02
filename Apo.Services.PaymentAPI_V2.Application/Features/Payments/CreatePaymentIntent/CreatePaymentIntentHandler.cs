using Apo.Services.PaymentAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.CreatePaymentIntent
{
    public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, PaymentDto>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStripeService _stripeService;
        private readonly IMapper _mapper;

        public CreatePaymentIntentHandler(IPaymentRepository repo, IStripeService stripeService, IMapper mapper)
        {
            _repo = repo;
            _stripeService = stripeService;
            _mapper = mapper;
        }

        public async Task<PaymentDto> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var (paymentIntentId, clientSecret) = await _stripeService.CreatePaymentIntentAsync(
                request.Amount, request.Currency, request.OrderId);

            var record = new PaymentRecord
            {
                OrderId = request.OrderId,
                UserId = request.UserId,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Pending,
                StripePaymentIntentId = paymentIntentId,
                StripeClientSecret = clientSecret,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(record);
            return _mapper.Map<PaymentDto>(created);
        }
    }
}
