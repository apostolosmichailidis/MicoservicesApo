using Apo.Services.EmailAPI_V2.Application.Common.Exceptions;
using AutoMapper;
using MediatR;

namespace Apo.Services.EmailAPI_V2.Application.Features.Emails.GetById
{
    public class GetEmailByIdHandler : IRequestHandler<GetEmailByIdQuery, EmailDto>
    {
        private readonly IEmailRepository _repo;
        private readonly IMapper _mapper;

        public GetEmailByIdHandler(IEmailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EmailDto> Handle(GetEmailByIdQuery request, CancellationToken cancellationToken)
        {
            var record = await _repo.GetByIdAsync(request.Id);
            if (record is null) throw new NotFoundException($"Email {request.Id} not found");
            return _mapper.Map<EmailDto>(record);
        }
    }
}
