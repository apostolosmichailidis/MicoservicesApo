using Apo.Services.EmailAPI_V2.Application;
using Apo.Services.EmailAPI_V2.Application.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Apo.Services.EmailAPI_V2.Infrastructure
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpOptions _options;

        public SmtpEmailService(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(string recipientEmail, string? recipientName, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName ?? recipientEmail, recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.Auto);

            if (!string.IsNullOrEmpty(_options.Username))
                await client.AuthenticateAsync(_options.Username, _options.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
