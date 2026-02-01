using Apo.Service.ShoppingCartAPI.Data;
using Apo.Service.ShoppingCartAPI.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public CartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        //[HttpPost("CartUpsert")]
        //public Task<ResponseDto> Upsert([FromBody] CartDto cartDto)
        //{
        //    try
        //    {
        //        // Logic to add or update cart in the database would go here
        //        _response.Result = cartDto; // For demonstration, returning the received cartDto
        //        return Ok(_response);
        //    }
        //    catch (Exception exc)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = exc.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, _response);
        //    }
        //}

    }
}
