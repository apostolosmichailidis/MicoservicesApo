using Apo.Services.EmailAPI_V2.Domain;

namespace Apo.Services.EmailAPI_V2.Application
{
    public interface IEmailRepository
    {
        Task<IEnumerable<EmailRecord>> GetAllAsync();
        Task<EmailRecord?> GetByIdAsync(int id);
        Task<IEnumerable<EmailRecord>> GetByRecipientEmailAsync(string recipientEmail);
        Task<EmailRecord> CreateAsync(EmailRecord record);
        Task<EmailRecord> UpdateAsync(EmailRecord record);
    }
}
