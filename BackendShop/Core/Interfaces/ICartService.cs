using BackendShop.Core.Dto.Cart;
using BackendShop.Core.Dto.Order;

namespace BackendShop.Core.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, CartItemDto cartItemDto);
        Task RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<IEnumerable<OrderDto>> GetOrdersAsync(int userId);
    }
}
