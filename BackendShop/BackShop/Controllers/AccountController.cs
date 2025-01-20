using BackendShop.Core.Dto.Account;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;        

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var token = await _accountService.LoginAsync(model);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                var message = await _accountService.RegisterAsync(model);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _accountService.GetAllAdminsAsync();
            return Ok(admins);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        //{
        //    try
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user == null)
        //            return BadRequest("Не вірно вказано дані");

        //        if (!await userManager.CheckPasswordAsync(user, model.Password))
        //            return BadRequest("Не вірно вказано дані");

        //        var token = await jwtTokenService.CreateTokenAsync(user);
        //        return Ok(new { token });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        //{
        //    try
        //    {
        //        var existingUser = await userManager.FindByEmailAsync(model.Email);
        //        if (existingUser != null)
        //            return BadRequest("Користувач з таким email вже існує");

        //        var user = new UserEntity
        //        {
        //            Firstname = model.FirstName,
        //            Lastname = model.LastName,
        //            Email = model.Email,
        //            UserName = model.Email
        //        };

        //        var result = await userManager.CreateAsync(user, model.Password);
        //        if (!result.Succeeded)
        //            return BadRequest(result.Errors);

        //        await userManager.AddToRoleAsync(user, "User");
        //        return Ok(new { message = "Користувач успішно зареєстрований" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("users")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await userManager.GetUsersInRoleAsync("User");
        //    //var users = await userManager.Users.ToListAsync(); // Повертаємо всіх користувачів

        //    return Ok(users.Select(user => new
        //    {
        //        user.Id,
        //        user.Firstname,
        //        user.Lastname,
        //        user.Email,
        //        user.Image
        //    }));
        //}

        //[HttpGet("admins")]
        //public async Task<IActionResult> GetAllAdmins()
        //{
        //    var admins = await userManager.GetUsersInRoleAsync("Admin");
        //    return Ok(admins.Select(admin => new
        //    {
        //        admin.Id,
        //        admin.Firstname,
        //        admin.Lastname,
        //        admin.Email,
        //        admin.Image
        //    }));
        //}

    }
}
