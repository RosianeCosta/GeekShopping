using GeekShopping.ProductAPI.Data.ValueObject;

namespace GeekShopping.ProductAPI.Repository
{
    public interface IProductRepository
    {
        IEnumerable<ProductVO> FindAll();  
        ProductVO FindById(long id);  
        ProductVO Create(ProductVO vo);  
        ProductVO Update(ProductVO vo);  
        bool Delete(long id);  
    }
}