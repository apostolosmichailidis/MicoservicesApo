using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.GetById
{
    public record GetEmailByIdQuery(int Id) : IRequest<EmailDto>;
}
