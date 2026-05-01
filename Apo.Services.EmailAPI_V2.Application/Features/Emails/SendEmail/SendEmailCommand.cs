using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.SendEmail
{
    public record SendEmailCommand(
        string RecipientEmail,
        string? RecipientName,
        string Subject,
        string Body) : IRequest<EmailDto>;
}
