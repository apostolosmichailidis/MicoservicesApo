using Apo.Services.ProductAPI_V2.Domain;

namespace Apo.Services.ProductAPI_V2.Application
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product coupon);
        Task<Product> UpdateAsync(Product coupon);
        Task<bool> DeleteAsync(int id);
    }
}
