using Apo.Service.EmailAPI_v2.Models;
using Apo.Services.EmailAPI_V2.Application.Features.Emails.GetById;
using Apo.Services.EmailAPI_V2.Application.Features.Emails.GetByRecipient;
using Apo.Services.EmailAPI_V2.Application.Features.Emails.RetryFailed;
using Apo.Services.EmailAPI_V2.Application.Features.Emails.SendEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.EmailAPI_V2.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest request)
            => ApiResponse(await _mediator.Send(new SendEmailCommand(
                request.RecipientEmail, request.RecipientName, request.Subject, request.Body)));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => ApiResponse(await _mediator.Send(new GetEmailByIdQuery(id)));

        [HttpGet("recipient/{recipientEmail}")]
        public async Task<IActionResult> GetByRecipient(string recipientEmail)
            => ApiResponse(await _mediator.Send(new GetEmailsByRecipientQuery(recipientEmail)));

        [HttpPost("retry/{id:int}")]
        public async Task<IActionResult> Retry(int id)
            => ApiResponse(await _mediator.Send(new RetryFailedEmailCommand(id)));
    }
}
