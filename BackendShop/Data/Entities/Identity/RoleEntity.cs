using Microsoft.AspNetCore.Identity;

namespace BackendShop.Data.Entities.Identity
{
    public class RoleEntity : IdentityRole<int>
    {
        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
