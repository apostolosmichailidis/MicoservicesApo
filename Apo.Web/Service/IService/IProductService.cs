using Apo.Web.Models;

namespace Apo.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductByNameAsync(string productName);
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> CreateUpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
