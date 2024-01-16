using AutoMapper;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Data.ValueObject;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IMapper _mapper;
        public ProductRepository(MySqlContext mySqlContext, IMapper mapper)
        {
            _mySqlContext = mySqlContext;
            _mapper = mapper;
        }
        public IEnumerable<ProductVO> FindAll()
        {
            List<Product> products = _mySqlContext.Product.ToList();
            return _mapper.Map<List<ProductVO>>(products);
        }

        public ProductVO FindById(long id)
        {
            Product product = new Product();
            try
            {

                product = _mySqlContext.Product.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var emss = ex.Message;
            }
            return _mapper.Map<ProductVO>(product);
        }

        public ProductVO Create(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            try 
            {
                _mySqlContext.Product.Add(product);
            }catch (Exception ex) 
            {
                var mess = ex.Message;
            }

            return _mapper.Map<ProductVO>(product);
        }

        public ProductVO Update(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _mySqlContext.Product.Update(product);

            return _mapper.Map<ProductVO>(product); ;
        }

        public bool Delete(long id)
        {
            try
            {
                Product product = _mySqlContext.Product.Where(x => x.Id == id).FirstOrDefault();

                if (product == null)
                    return false;

                _mySqlContext.Product.Remove(product);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}