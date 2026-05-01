using Apo.Services.EmailAPI_V2.Domain;
using AutoMapper;
using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.SendEmail
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, EmailDto>
    {
        private readonly IEmailRepository _repo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public SendEmailHandler(IEmailRepository repo, IEmailService emailService, IMapper mapper)
        {
            _repo = repo;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<EmailDto> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var record = new EmailRecord
            {
                RecipientEmail = request.RecipientEmail,
                RecipientName = request.RecipientName,
                Subject = request.Subject,
                Body = request.Body,
                Status = EmailStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            record = await _repo.CreateAsync(record);

            try
            {
                await _emailService.SendAsync(record.RecipientEmail, record.RecipientName, record.Subject, record.Body);
                record.Status = EmailStatus.Sent;
                record.SentAt = DateTime.UtcNow;
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
