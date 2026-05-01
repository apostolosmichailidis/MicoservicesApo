using Apo.Services.EmailAPI_V2.Application;
using Apo.Services.EmailAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Apo.Services.EmailAPI_V2.Infrastructure
{
    public class EmailRepository : IEmailRepository
    {
        private readonly AppDbContext _db;

        public EmailRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<EmailRecord>> GetAllAsync()
            => await _db.EmailRecords.ToListAsync();

        public async Task<EmailRecord?> GetByIdAsync(int id)
            => await _db.EmailRecords.FirstOrDefaultAsync(e => e.EmailId == id);

        public async Task<IEnumerable<EmailRecord>> GetByRecipientEmailAsync(string recipientEmail)
            => await _db.EmailRecords
                .Where(e => e.RecipientEmail.ToLower() == recipientEmail.ToLower())
                .ToListAsync();

        public async Task<EmailRecord> CreateAsync(EmailRecord record)
        {
            _db.EmailRecords.Add(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public async Task<EmailRecord> UpdateAsync(EmailRecord record)
        {
            _db.EmailRecords.Update(record);
            await _db.SaveChangesAsync();
            return record;
        }
    }
}
