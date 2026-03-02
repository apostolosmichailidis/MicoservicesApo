using Apo.Services.PaymentAPI_V2.Domain;

namespace Apo.Services.PaymentAPI_V2.Application
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<PaymentRecord>> GetAllAsync();
        Task<PaymentRecord?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentRecord>> GetByOrderIdAsync(string orderId);
        Task<PaymentRecord?> GetByStripePaymentIntentIdAsync(string paymentIntentId);
        Task<PaymentRecord> CreateAsync(PaymentRecord record);
        Task<PaymentRecord> UpdateAsync(PaymentRecord record);
    }
}
