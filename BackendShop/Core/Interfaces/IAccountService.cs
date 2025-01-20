using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Account;
using BackendShop.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendShop.Core.Interfaces
{
    public interface IAccountService
    {
        Task<string> LoginAsync(LoginViewModel model);
        Task<string> RegisterAsync(RegisterViewModel model);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetAllAdminsAsync();
    }
}
