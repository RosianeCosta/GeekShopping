using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/coupon";
        public CouponRepository(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        //public async Task<CouponVO> GetCoupon(string couponCode, string token)
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = await _client.GetAsync($"/api/v1/coupon/{couponCode}");
        //    var responseMeu = await _client.GetAsync($"{BasePath}/{couponCode}");

        //    var content = await response.Content.ReadAsStringAsync();
            
        //    if (response.StatusCode != HttpStatusCode.OK) 
        //        return new CouponVO();
            
        //    return JsonSerializer.Deserialize<CouponVO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}

        public async Task<CouponVO> GetCoupon(string couponCode, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{BasePath}/{couponCode}");
            if (response.StatusCode != HttpStatusCode.OK) return new CouponVO();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CouponVO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //return await response.ReadContentAs<CouponVO>();
        }
    }
}