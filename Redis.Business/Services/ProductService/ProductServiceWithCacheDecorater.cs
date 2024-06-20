using Redis.Business.Models;
using Redis.Cache.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Redis.Business.Services.ProductService
{
    public class ProductServiceWithCacheDecorater : IProductService
    {
        private const string productKey = "ProductCaches";
        private readonly IProductService _productService;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        public ProductServiceWithCacheDecorater(IProductService productService, RedisService redisService)
        {
            _productService = productService;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDatabase(2);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var newProduct = await _productService.CreateProductAsync(product);
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                object value = await _cacheRepository.HashSetAsync(productKey, newProduct.Id.ToString(), JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetProductAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value!);
                products.Add(product!);
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid Id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, Id.ToString());
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product!) : null;
            }

            var products = LoadToCacheFromDbAsync();

            return products.Result.FirstOrDefault()!;
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productService.GetProductAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id.ToString(), JsonSerializer.Serialize(p));
            });

            return products;
        }
    }
}
