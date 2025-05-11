using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BookingApp.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var users = await _userService.GetUsersAsync();
            _logger.LogInformation("Получен список всех пользователей.");
            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTOPublic>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с id {Id} не найден.", id);
                return NotFound($"Пользователь с id={id} не найден.");
            }

            _logger.LogInformation("Получен пользователь с id {Id}.", id);
            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] UserUpdateDTO userDto)
        {
            if (userDto == null || id != userDto.Id)
            {
                _logger.LogWarning("Некорректные данные при обновлении пользователя id {Id}.", id);
                return BadRequest("Некорректные данные о пользователе.");
            }

            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                _logger.LogWarning("Пользователь с id {Id} не найден для обновления.", id);
                return NotFound($"Пользователь с id={id} не найден.");
            }

            _logger.LogInformation("Обновлен пользователь с id {Id}.", id);
            return Ok(updatedUser);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound("Такой пользователь не найден.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTOPublic>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Не удалось удалить пользователя с id={Id}. Не найден.", userId);
                return Unauthorized("Не удалось определить текущего пользователя.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation("Удален пользователь с id={Id}.", userId);
                return NotFound("Пользователь не найден.");
            }

            return Ok(user);
        }
    }
}
