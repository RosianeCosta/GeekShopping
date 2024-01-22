using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));
        }

        public async Task<IActionResult> ProductIndex()
        {
            var products = await _productService.FindAllProducts();
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(model);
                if (response != null) RedirectToAction("ProductIndex");
            }

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(int  id)
        {
            var model = _productService.FindAllProductById(id).Result;
            if (model != null) { return View(model); }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProduct(model);
                if (response != null) RedirectToAction("ProductIndex");
            }

            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            var model = _productService.FindAllProductById(id).Result;
            if (model != null) { return View(model); }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductModel model)
        {
            var product = _productService.DeleteProductById(model.Id).Result;
            if (product) RedirectToAction("ProductIndex");
            
            return NotFound();
        }
    }
}
