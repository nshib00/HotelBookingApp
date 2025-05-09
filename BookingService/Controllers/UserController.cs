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

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTOPublic>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Такой пользователь не найден.");
            }
            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] UserDTO userDto)
        {
            if (userDto == null || id != userDto.Id)
            {
                return BadRequest("Некорректные данные.");
            }

            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                return NotFound("Такой пользователь не найден.");
            }

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
                return Unauthorized("Не удалось определить текущего пользователя.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            return Ok(user);
        }
    }
}
