using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookingApp.Api.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _hotelService;
        private readonly RoomService _roomService;
        private readonly ILogger<HotelController> _logger;

        public HotelController(HotelService hotelService, RoomService roomService, ILogger<HotelController> logger)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<HotelDTO>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var hotelsPaged = await _hotelService.GetHotelsPagedAsync(page, pageSize);
            _logger.LogInformation("Получен список всех отелей: страница {page}, размер страницы: {pageSize}", page, pageSize);
            return Ok(hotelsPaged);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDTO>> Get(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                _logger.LogWarning("Отель с id {Id} не найден.", id);
                return NotFound($"Отель с id={id} не найден.");
            }

            _logger.LogInformation("Получен отель с id {Id}.", id);
            return Ok(hotel);
        }


        [HttpGet("{id}/Rooms")]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> GetRooms(int id)
        {
            HotelDTO? hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                _logger.LogWarning("Отель с id={Id} не найден.", id);
                return NotFound($"Отель не найден.");
            }

            var hotelRooms = await _roomService.GetHotelRoomsAsync(hotel);
            _logger.LogInformation("Получен список номеров отеля с id={Id}.", id);
            return Ok(hotelRooms);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HotelDTO hotelDto)
        {
            if (hotelDto == null)
            {
                _logger.LogWarning("Попытка создать отель с некорректными данными.");
                return BadRequest("Некорректные данные об отеле.");
            }

            var createdHotel = await _hotelService.AddHotelAsync(hotelDto);
            _logger.LogInformation("Создан отель с id {Id}.", createdHotel.Id);
            return CreatedAtAction(nameof(Get), new { id = createdHotel.Id }, createdHotel);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] HotelDTO hotelDto)
        {
            if (hotelDto == null || id != hotelDto.Id)
            {
                _logger.LogWarning("Некорректные данные при обновлении отеля id {Id}.", id);
                return BadRequest("Некорректные данные об отеле.");
            }

            var updatedHotel = await _hotelService.UpdateHotelAsync(hotelDto);
            if (updatedHotel == null)
            {
                _logger.LogWarning("Отель с id {Id} не найден для обновления.", id);
                return NotFound($"Отель с id={id} не найден.");
            }

            _logger.LogInformation("Обновлен отель с id {Id}.", id);
            return Ok(updatedHotel);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _hotelService.DeleteHotelAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Не удалось удалить отель с id {Id}. Не найден.", id);
                return NotFound($"Отель с id={id} не найден.");
            }

            _logger.LogInformation("Удален отель с id {Id}.", id);
            return NoContent();
        }
    }
}