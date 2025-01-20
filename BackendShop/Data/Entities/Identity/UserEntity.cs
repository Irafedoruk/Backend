using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [StringLength(255)]
        public string? Image { get; set; }
        [StringLength(100)]
        public string? Lastname { get; set; }
        [StringLength(100)]
        public string? Firstname { get; set; }
        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
        public DateTime? Birthdate { get; set; }
        //public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        //public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
