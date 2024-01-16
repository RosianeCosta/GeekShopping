using Microsoft.AspNetCore.Mvc;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Data.ValueObject;

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
        public IActionResult FindById(long id) 
        {
            var product = _productRepository.FindById(id);
            if (product == null) return NotFound();

            return Ok(product); 
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            var products = _productRepository.FindAll();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create(ProductVO vo)
        {
            if (vo == null) return BadRequest();
            var product = _productRepository.Create(vo);

            return Ok(product);
        }

        [HttpPut]
        public IActionResult Update(ProductVO vo)
        {
            if (vo == null) return BadRequest();
            var product = _productRepository.Update(vo);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var status = _productRepository.Delete(id);
            if(!status) return BadRequest();    
            return Ok(status);
        }
    }
}