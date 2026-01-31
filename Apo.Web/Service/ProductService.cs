using Apo.Web.Models;
using Apo.Web.Service.IService;

namespace Apo.Web.Service
{
    public class ProductService : IProductService
    {
        private IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateUpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.POST,
                Data = productDto,
                Url = Apo.Web.Utility.SD.ProductAPIBase + $"/api/product"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.DELETE,
                Url = Apo.Web.Utility.SD.ProductAPIBase + $"/api/product/{id}"
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET,
                Url = Apo.Web.Utility.SD.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET, 
                Url = Apo.Web.Utility.SD.ProductAPIBase + $"/api/product/{id}" 
            });
        }

        public async Task<ResponseDto?> GetProductByNameAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET,
                Url = Apo.Web.Utility.SD.ProductAPIBase + $"/api/product/GetByName/{productName}"
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.PUT,
                Data = productDto,
                Url = Apo.Web.Utility.SD.ProductAPIBase + $"/api/product"
            });
        }
    }
}
