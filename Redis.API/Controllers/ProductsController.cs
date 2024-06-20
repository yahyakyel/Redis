using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Business.Models;
using Redis.Business.Services.ProductService;

namespace Redis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var Products = await _productService.CreateProductAsync(product);
            return Ok(Products);
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productService.GetProductAsync());
        }
        [HttpGet("GetByIdProduct")]
        public async Task<IActionResult> GetByIdProduct(Guid Id)
        {
            return Ok(await _productService.GetProductByIdAsync(Id));
        }
    }
}
