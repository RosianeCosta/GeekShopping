using GeekShopping.CardAPI.Messages;
using GeekShopping.CardAPI.RabbitMQSender;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;
        private ICouponRepository _couponrepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository repository, 
            IRabbitMQMessageSender rabbitMQMessageSender,
            ICouponRepository couponrepository)
        {
            _cartRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
            _couponrepository = couponrepository ?? throw new ArgumentNullException(nameof(couponrepository));
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(string userId)
        {
            var cart = await _cartRepository.FindCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart/{id}")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
            if (!status == null) return NotFound();
            return Ok(status);
        }       
        
        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status == null) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var cart = await _cartRepository.FindCartByUserId(vo.UserId);
            if (cart == null) return NotFound();

            if (!string.IsNullOrEmpty(vo.CouponCode)) 
            {
                CouponVO coupon = await _couponrepository.GetCoupon(vo.CouponCode, token);
                if(vo.DiscountAmount != coupon.DiscountAmount) 
                {
                    //412 = Pre condition failed - Mudou as condições entre a primeira requisição e a confirmação de compra 
                    return StatusCode(412);
                }
            }

            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            //TASK RabbitMQ
            _rabbitMQMessageSender.SendMessage(vo, "Checkoutqueue");

            return Ok(vo);
        }
    }
}