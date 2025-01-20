using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Зв'язок з товаром
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Ціна товару на момент замовлення
        public Order Order { get; set; } // Зв'язок із замовленням
        public int OrderId { get; set; }
    }
}
