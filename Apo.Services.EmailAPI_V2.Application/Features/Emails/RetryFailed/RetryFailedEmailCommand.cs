using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.RetryFailed
{
    public record RetryFailedEmailCommand(int EmailId) : IRequest<EmailDto>;
}
