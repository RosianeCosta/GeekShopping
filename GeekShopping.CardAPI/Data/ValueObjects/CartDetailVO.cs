namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class CartDetailVO 
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public virtual CartHeaderVO CartHeader { get; set; } = new CartHeaderVO();
        public long ProductId { get; set; }
        public virtual ProductVo Product { get; set; } = new ProductVo();
        public int Count { get; set; }
    }
}