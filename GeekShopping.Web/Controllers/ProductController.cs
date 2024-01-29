using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using GeekShopping.Web.Util;
using Microsoft.AspNetCore.Authentication;

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
            var products = await _productService.FindAllProducts(string.Empty);
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductCreate(ProductViewModel model)
        {
            if(ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.CreateProduct(model, token);
                if (response != null) RedirectToAction("ProductIndex");
            }

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(int  id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var model = _productService.FindAllProductById(id, token).Result;
            if (model != null) { return View(model); }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductUpdate(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.UpdateProduct(model, token);
                if (response != null) return RedirectToAction("ProductIndex");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var model = _productService.FindAllProductById(id, token).Result;
            if (model != null) { return View(model); }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(ProductViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var product = _productService.DeleteProductById(model.Id, token);
            if (product.Result) return RedirectToAction("ProductIndex");
            
            return NotFound();
        }
    }
}
