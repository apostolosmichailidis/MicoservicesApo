using Apo.Services.PaymentAPI_V2.Application;
using Apo.Services.PaymentAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Apo.Services.PaymentAPI_V2.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PaymentRecord>> GetAllAsync()
            => await _db.PaymentRecords.ToListAsync();

        public async Task<PaymentRecord?> GetByIdAsync(int id)
            => await _db.PaymentRecords.FirstOrDefaultAsync(p => p.PaymentId == id);

        public async Task<IEnumerable<PaymentRecord>> GetByOrderIdAsync(string orderId)
            => await _db.PaymentRecords.Where(p => p.OrderId == orderId).ToListAsync();

        public async Task<PaymentRecord?> GetByStripePaymentIntentIdAsync(string paymentIntentId)
            => await _db.PaymentRecords.FirstOrDefaultAsync(p => p.StripePaymentIntentId == paymentIntentId);

        public async Task<PaymentRecord> CreateAsync(PaymentRecord record)
        {
            _db.PaymentRecords.Add(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public async Task<PaymentRecord> UpdateAsync(PaymentRecord record)
        {
            _db.PaymentRecords.Update(record);
            await _db.SaveChangesAsync();
            return record;
        }
    }
}
