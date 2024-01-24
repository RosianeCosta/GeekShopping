using Microsoft.AspNetCore.Mvc;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Data.ValueObject;
using Microsoft.AspNetCore.Authorization;
using GeekShopping.ProductAPI.Util;

namespace GeekShopping.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> FindById(long id) 
        {
            var product = await _productRepository.FindById(id);
            if (product == null) return NotFound();

            return Ok(product); 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FindAll()
        {
            var products = await _productRepository.FindAll();
            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductVO vo)
        {
            if (vo == null) return BadRequest();
            var product = await _productRepository.Create(vo);

            return Ok(product);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(ProductVO vo)
        {
            if (vo == null) return BadRequest();
            var product = await _productRepository.Update(vo);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(long id)
        {
            var status = await _productRepository.Delete(id);
            if(!status) return BadRequest();    
            return Ok(status);
        }
    }
}