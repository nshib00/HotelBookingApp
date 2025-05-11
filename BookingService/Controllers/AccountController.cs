using BookingApp.Api.Models;
using BookingApp.Api.Services;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace BookingApp.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                _logger.LogInformation("Пользователь {Email} успешно зарегистрирован.", user.Email);
                return Ok("Регистрация прошла успешно!");
            }

            _logger.LogWarning("Ошибка регистрации пользователя {Email}: {Errors}", user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest("Ошибка регистрации: " + string.Join(", ", result.Errors.Select(e => e.Description)) + ".");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                _logger.LogWarning("Попытка входа с несуществующим email: {Email}", model.Email);
                return Unauthorized("Пользователь не найден.");
            }

            if (user.UserName != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var token = TokenManager.GenerateJwtToken(user, _userManager, _configuration);
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = roles.FirstOrDefault() ?? "user";

                    _logger.LogInformation("Пользователь {Email} успешно вошел в систему.", user.Email);
                    return Ok(new
                    {
                        token,
                        email = user.Email,
                        userRole
                    });
                }
            }

            _logger.LogWarning("Неудачная попытка входа для email: {Email}", model.Email);
            return Unauthorized("Неверный email или пароль.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Пользователь вышел из системы.");
            return Ok("Выход выполнен успешно.");
        }
    }
}
