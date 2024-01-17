using GeekShopping.Web.Util;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/product";
        public async Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            var response = await _client.GetAsync(BasePath);
            return await response.ReadContent<List<ProductModel>>();
        }

        public async Task<ProductModel> FindAllProductById(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContent<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel model)
        {
            var response = await _client.PostAsJson(BasePath, model);
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao consultar Product API");

            return await response.ReadContent<ProductModel>();
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var response = await _client.PutAsJson(BasePath, model);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao consultar Product API");

            return await response.ReadContent<ProductModel>();
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao consultar Product API");

            return await response.ReadContent<bool>();
        }
    }
}