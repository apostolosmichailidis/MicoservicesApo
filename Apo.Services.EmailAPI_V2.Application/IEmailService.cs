namespace Apo.Services.EmailAPI_V2.Application
{
    public interface IEmailService
    {
        Task SendAsync(string recipientEmail, string? recipientName, string subject, string body);
    }
}
