namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class CartVO
    {
        public long Id { get; set; }
        public CartHeaderVO CartHeader { get; set; } = new CartHeaderVO();
        public IEnumerable<CartDetailVO> CartDetails { get; set; } = new List<CartDetailVO>();
    }
}