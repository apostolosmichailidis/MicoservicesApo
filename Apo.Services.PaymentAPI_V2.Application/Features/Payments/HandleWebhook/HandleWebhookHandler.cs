using Apo.Services.PaymentAPI_V2.Domain;
using MediatR;

namespace Apo.Services.PaymentAPI_V2.Application.Features.Payments.HandleWebhook
{
    public class HandleWebhookHandler : IRequestHandler<HandleWebhookCommand, bool>
    {
        private readonly IPaymentRepository _repo;

        public HandleWebhookHandler(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(HandleWebhookCommand request, CancellationToken cancellationToken)
        {
            var record = await _repo.GetByStripePaymentIntentIdAsync(request.PaymentIntentId);
            if (record is null) return false;

            record.Status = request.EventType switch
            {
                "succeeded" => PaymentStatus.Succeeded,
                "failed" => PaymentStatus.Failed,
                _ => record.Status
            };
            record.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(record);
            return true;
        }
    }
}
