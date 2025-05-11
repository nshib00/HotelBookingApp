using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookingApp.Api.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly ILogger<RoomController> _logger;

        public RoomController(RoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> Get(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                _logger.LogWarning("Номер с id {Id} не найден.", id);
                return NotFound($"Номер с id={id} не найден.");
            }

            _logger.LogInformation("Получен номер с id {Id}.", id);
            return Ok(room);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                _logger.LogWarning("Попытка создать номер с некорректными данными.");
                return BadRequest("Некорректные данные о номере.");
            }

            var createdRoom = await _roomService.AddRoomAsync(roomDto);
            _logger.LogInformation("Создан номер с id {Id}.", createdRoom.Id);
            return CreatedAtAction(nameof(Get), new { id = createdRoom.Id }, createdRoom);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RoomDTO roomDto)
        {
            if (roomDto == null || id != roomDto.Id)
            {
                _logger.LogWarning("Некорректные данные при обновлении номера id {Id}.", id);
                return BadRequest("Некорректные данные о номере.");
            }

            var updatedRoom = await _roomService.UpdateRoomAsync(roomDto);
            if (updatedRoom == null)
            {
                _logger.LogWarning("Номер с id {Id} не найден для обновления.", id);
                return NotFound($"Номер с id={id} не найден.");
            }

            _logger.LogInformation("Обновлен номер с id {Id}.", id);
            return Ok(updatedRoom);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteRoomAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Не удалось удалить номер с id {Id}. Не найден.", id);
                return NotFound($"Номер с id={id} не найден.");
            }

            _logger.LogInformation("Удален номер с id {Id}.", id);
            return NoContent();
        }
    }
}
