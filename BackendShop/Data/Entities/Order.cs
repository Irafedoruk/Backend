using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BackendShop.Constants;
using BackendShop.Data.Entities.Identity;

namespace BackendShop.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Ідентифікатор користувача
        public UserEntity User { get; set; } // Навігаційна властивість для користувача
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Наприклад, Pending, Completed, Canceled
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalPrice { get; set; }
    }
}
