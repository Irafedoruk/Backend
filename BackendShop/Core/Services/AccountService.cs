using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Account;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendShop.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountService(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception("Невірний email або пароль");

            return await _jwtTokenService.CreateTokenAsync(user);
        }

        public async Task<string> RegisterAsync(RegisterViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception("Користувач з таким email вже існує");

            var user = new UserEntity
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");
            return "Користувач успішно зареєстрований";
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email
            });
        }

        public async Task<IEnumerable<UserDto>> GetAllAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(admin => new UserDto
            {
                Id = admin.Id,
                Firstname = admin.Firstname,
                Lastname = admin.Lastname,
                Email = admin.Email
            });
        }
    }
}
