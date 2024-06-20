using Redis.Business.Models;

namespace Redis.Business.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetProductAsync();
        Task<Product> GetProductByIdAsync(Guid Id);
        Task<Product> CreateProductAsync(Product product);
    }
}
