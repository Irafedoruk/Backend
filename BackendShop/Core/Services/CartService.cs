using BackendShop.Core.Dto.Cart;
using BackendShop.Core.Dto.Order;
using BackendShop.Core.Interfaces;
using BackendShop.Data;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendShop.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ShopDbContext _context;

        public CartService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return new CartDto();

            var cartDto = new CartDto
            {
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                TotalPrice = cart.Items.Sum(i => i.Quantity * GetProductPrice(i.ProductId)) // Симуляція отримання ціни
            };

            return cartDto;
        }

        public async Task AddToCartAsync(int userId, CartItemDto cartItemDto)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDto.ProductId);
            if (cartItem == null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity
                });
            }
            else
            {
                cartItem.Quantity += cartItemDto.Quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            cart.Items.Clear();
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Cart is empty");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = Constants.OrderStatus.Pending,
                Items = createOrderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = GetProductPrice(i.ProductId)
                }).ToList(),
                TotalPrice = createOrderDto.Items.Sum(i => i.Quantity * GetProductPrice(i.ProductId))
            };

            _context.Orders.Add(order);
            cart.Items.Clear();

            await _context.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                UserId = userId.ToString(),
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId.ToString(),
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalPrice = o.TotalPrice,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });
        }

        private decimal GetProductPrice(int productId)
        {
            // Реалізуйте отримання ціни товару з бази даних
            return 100m; // Тимчасова симуляція
        }
    }
}
