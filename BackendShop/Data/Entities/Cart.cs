using BackendShop.Data.Entities.Identity;

namespace BackendShop.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Ідентифікатор користувача
        public UserEntity User { get; set; } // Навігаційна властивість для користувача
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
