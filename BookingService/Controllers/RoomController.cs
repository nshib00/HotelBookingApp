using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> Get(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Некорректные данные номера отеля.");
            }

            var createdRoom = await _roomService.AddRoomAsync(roomDto);
            return CreatedAtAction(nameof(Get), new { id = createdRoom.Id }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RoomDTO roomDto)
        {
            if (roomDto == null || id != roomDto.Id)
            {
                return BadRequest("Некорректные данные номера отеля.");
            }

            var updatedRoom = await _roomService.UpdateRoomAsync(roomDto);
            if (updatedRoom == null)
            {
                return NotFound($"Номер с ID {id} не найден.");
            }

            return Ok(updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteRoomAsync(id);
            if (!deleted)
            {
                return NotFound($"Номер с ID {id} не найден.");
            }

            return NoContent();
        }
    }
}
