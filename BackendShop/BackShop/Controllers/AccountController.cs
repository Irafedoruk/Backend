using BackendShop.Core.Dto.Account;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<UserEntity> userManager,
        IJwtTokenService jwtTokenService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest("Не вірно вказано дані");

                if (!await userManager.CheckPasswordAsync(user, model.Password))
                    return BadRequest("Не вірно вказано дані");

                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.GetUsersInRoleAsync("User");
            //var users = await userManager.Users.ToListAsync(); // Повертаємо всіх користувачів
            
            return Ok(users.Select(user => new
            {
                user.Id,
                user.Firstname,
                user.Lastname,
                user.Email,
                user.Image
            }));
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            return Ok(admins.Select(admin => new
            {
                admin.Id,
                admin.Firstname,
                admin.Lastname,
                admin.Email,
                admin.Image
            }));
        }

    }
}
