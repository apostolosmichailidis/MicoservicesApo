using Apo.Service.PaymentAPI_v2.Models;
using Apo.Services.PaymentAPI_V2.Application;
using Apo.Services.PaymentAPI_V2.Application.Features.Payments.CreatePaymentIntent;
using Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetById;
using Apo.Services.PaymentAPI_V2.Application.Features.Payments.GetByOrderId;
using Apo.Services.PaymentAPI_V2.Application.Features.Payments.HandleWebhook;
using Apo.Services.PaymentAPI_V2.Application.Features.Payments.Refund;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.PaymentAPI_V2.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStripeService _stripeService;

        public PaymentController(IMediator mediator, IStripeService stripeService)
        {
            _mediator = mediator;
            _stripeService = stripeService;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
            => ApiResponse(await _mediator.Send(new CreatePaymentIntentCommand(
                request.OrderId, request.UserId, request.Amount, request.Currency)));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => ApiResponse(await _mediator.Send(new GetPaymentByIdQuery(id)));

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(string orderId)
            => ApiResponse(await _mediator.Send(new GetPaymentsByOrderIdQuery(orderId)));

        [HttpPost("refund/{id:int}")]
        public async Task<IActionResult> Refund(int id)
            => ApiResponse(await _mediator.Send(new RefundPaymentCommand(id)));

        /// <summary>
        /// Stripe webhook endpoint — receives payment events directly from Stripe.
        /// Must remain unauthenticated; Stripe signature is verified internally.
        /// </summary>
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].ToString();

            var webhookEvent = _stripeService.ParseWebhookEvent(json, signature);
            if (webhookEvent.HasValue)
            {
                await _mediator.Send(new HandleWebhookCommand(
                    webhookEvent.Value.PaymentIntentId,
                    webhookEvent.Value.EventType));
            }

            return Ok();
        }
    }
}
