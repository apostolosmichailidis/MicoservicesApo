using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.GetByRecipient
{
    public record GetEmailsByRecipientQuery(string RecipientEmail) : IRequest<IEnumerable<EmailDto>>;
}
