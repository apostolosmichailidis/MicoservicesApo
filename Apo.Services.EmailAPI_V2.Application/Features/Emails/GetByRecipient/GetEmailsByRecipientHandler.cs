using AutoMapper;
using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.GetByRecipient
{
    public class GetEmailsByRecipientHandler : IRequestHandler<GetEmailsByRecipientQuery, IEnumerable<EmailDto>>
    {
        private readonly IEmailRepository _repo;
        private readonly IMapper _mapper;

        public GetEmailsByRecipientHandler(IEmailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmailDto>> Handle(GetEmailsByRecipientQuery request, CancellationToken cancellationToken)
        {
            var records = await _repo.GetByRecipientEmailAsync(request.RecipientEmail);
            return _mapper.Map<IEnumerable<EmailDto>>(records);
        }
    }
}
