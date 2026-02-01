using Apo.Service.ShoppingCartAPI.Models.Dto;

namespace Apo.Service.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
