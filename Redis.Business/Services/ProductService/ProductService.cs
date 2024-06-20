using Microsoft.EntityFrameworkCore;
using Redis.Business.Models;

namespace Redis.Business.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<Product>> GetProductAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid Id)
        {
            var Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (Product == null) 
            {
                return new Product();
            }
            return Product;
        }
    }
}
