using System.Collections.Generic;

namespace GeekShopping.CartAPI.Model
{
    public class Cart
    {
        public long Id { get; set; }
        public CartHeader CartHeader { get; set; } = new CartHeader();
        public IEnumerable<CartDetail> CartDetails { get; set; } = Enumerable.Empty<CartDetail>();
    }
}
