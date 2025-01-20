using BackendShop.Core.Dto.Cart;
using BackendShop.Core.Dto.Order;
using BackendShop.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = int.Parse(User.FindFirst("id").Value); // Отримуємо ID користувача
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _cartService.AddToCartAsync(userId, cartItemDto);
            return NoContent();
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _cartService.RemoveFromCartAsync(userId, productId);
            return NoContent();
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var order = await _cartService.CreateOrderAsync(userId, createOrderDto);
            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var orders = await _cartService.GetOrdersAsync(userId);
            return Ok(orders);
        }
    }
}
