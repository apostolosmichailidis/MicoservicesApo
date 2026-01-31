using Apo.Service.CouponAPI.Data;
using Apo.Service.CouponAPI.Models;
using Apo.Service.CouponAPI.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var coupons = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public IActionResult Get(string code)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                var res = _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon); ;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(coupon);
                var res = _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon); ;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var coupon = _db.Coupons.First(c => c.CouponId == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
                return Ok(_response);
            }
            catch (Exception exc)
            {
                _response.IsSuccess = false;
                _response.Message = exc.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
