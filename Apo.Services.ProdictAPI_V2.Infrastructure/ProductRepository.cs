using Apo.Services.ProductAPI_V2.Application;
using Apo.Services.ProductAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;


namespace Apo.Services.ProductAPI_V2.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _db.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _db.Products.FirstOrDefaultAsync(c => c.ProductId == id);

        public async Task<Product> CreateAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null) return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
