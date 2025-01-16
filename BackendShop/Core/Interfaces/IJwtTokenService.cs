using BackendShop.Data.Entities.Identity;

namespace BackendShop.Core.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}
