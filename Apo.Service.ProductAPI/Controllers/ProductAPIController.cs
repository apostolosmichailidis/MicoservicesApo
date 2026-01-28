using Apo.Service.ProductAPI.Models;
using Apo.Service.ProductAPI.Models.Dto;
using Apo.Service.ProductAPI.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
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
                var products = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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
                var product = _db.Products.FirstOrDefault(c => c.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
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
        [Route("GetByName/{name}")]
        public IActionResult Get(string name)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
                _response.Result = _mapper.Map<ProductDto>(product);
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
        public IActionResult Post([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _db.Products.Add(product);
                var res = _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product); ;
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
        public IActionResult Put([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _db.Products.Update(product);
                var res = _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product); ;
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
                var product = _db.Products.First(c => c.ProductId == id);
                _db.Products.Remove(product);
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
