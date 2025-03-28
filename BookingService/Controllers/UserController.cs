using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
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
                return NotFound($"Такой пользователь не найден.");
            }

            return NoContent();
        }
    }
}
