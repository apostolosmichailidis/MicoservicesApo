using Apo.Services.EmailAPI_V2.Application.Common.Exceptions;
using Apo.Services.EmailAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.RetryFailed
{
    public class RetryFailedEmailHandler : IRequestHandler<RetryFailedEmailCommand, EmailDto>
    {
        private readonly IEmailRepository _repo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public RetryFailedEmailHandler(IEmailRepository repo, IEmailService emailService, IMapper mapper)
        {
            _repo = repo;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<EmailDto> Handle(RetryFailedEmailCommand request, CancellationToken cancellationToken)
        {
            var record = await _repo.GetByIdAsync(request.EmailId);
            if (record is null) throw new NotFoundException($"Email {request.EmailId} not found");

            try
            {
                await _emailService.SendAsync(record.RecipientEmail, record.RecipientName, record.Subject, record.Body);
                record.Status = EmailStatus.Sent;
                record.SentAt = DateTime.UtcNow;
                record.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                record.Status = EmailStatus.Failed;
                record.ErrorMessage = ex.Message;
            }

            record = await _repo.UpdateAsync(record);
            return _mapper.Map<EmailDto>(record);
        }
    }
}
