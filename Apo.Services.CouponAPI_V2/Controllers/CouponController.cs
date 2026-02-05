using Apo.Services.CouponAPI_V2.Application.Features.Coupons;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.Create;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.Delete;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetAll;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetByCode;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.GetById;
using Apo.Services.CouponAPI_V2.Application.Features.Coupons.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Services.CouponAPI_V2.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CouponController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            =>  ApiResponse(await _mediator.Send(new GetAllCouponsQuery()));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => ApiResponse(Ok(await _mediator.Send(new GetCouponByIdQuery(id))));

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> Get(string code)
            => ApiResponse(Ok(await _mediator.Send(new GetCouponByCodeQuery(code))));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CouponDto dto)
            => ApiResponse(Ok(await _mediator.Send(new CreateCouponCommand(dto))));

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CouponDto dto)
            => ApiResponse(Ok(await _mediator.Send(new UpdateCouponCommand(dto))));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => ApiResponse(Ok(await _mediator.Send(new DeleteCouponCommand(id))));
    }
}
