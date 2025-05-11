using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingApp.Api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;
        private readonly UserService _userService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(BookingService bookingService, UserService userService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> Get()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Не удалось получить id текущего пользователя.");
                return NotFound($"Пользователь не найден.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с id {UserId} не найден.", userId);
                return NotFound($"Пользователь с id={userId} не найден.");
            }

            var bookings = await _bookingService.GetUserBookingsAsync(user);
            _logger.LogInformation("Получены бронирования для пользователя {UserId}.", userId);
            return Ok(bookings);
        }

        [HttpGet("{bookingId}")]
        public async Task<ActionResult<BookingDTO>> Get(int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            _logger.LogInformation("Получено бронирование с id {BookingId}.", bookingId);
            return Ok(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BookingCreateDTO bookingDto)
        {
            if (bookingDto == null)
            {
                _logger.LogWarning("Попытка создать бронирование с некорректными данными.");
                return BadRequest("Некорректные данные о бронировании.");
            }

            if (bookingDto.DateFrom > bookingDto.DateTo)
            {
                _logger.LogWarning("Некорректный период бронирования: {DateFrom} > {DateTo}.", bookingDto.DateFrom, bookingDto.DateTo);
                return BadRequest("Начало проживания не может быть позже конца проживания.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Не удалось получить id текущего пользователя.");
                return Unauthorized("Пользователь не найден.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с id {UserId} не найден.", userId);
                return NotFound($"Пользователь с id={userId} не найден.");
            }

            var newBookingDto = new BookingDTO
            {
                RoomId = bookingDto.RoomId,
                DateFrom = DateTime.SpecifyKind(bookingDto.DateFrom, DateTimeKind.Utc),
                DateTo = DateTime.SpecifyKind(bookingDto.DateTo, DateTimeKind.Utc),
                UserId = userId
            };

            var createdBooking = await _bookingService.AddBookingAsync(newBookingDto);
            _logger.LogInformation("Создано бронирование с id {BookingId} для пользователя {UserId}.", createdBooking.Id, userId);
            return CreatedAtAction(nameof(Get), new { bookingId = createdBooking.Id }, createdBooking);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null || id != bookingDto.Id)
            {
                _logger.LogWarning("Некорректные данные при обновлении бронирования id {Id}.", id);
                return BadRequest("Некорректные данные о бронировании.");
            }

            var updatedBooking = await _bookingService.UpdateBookingAsync(bookingDto);
            if (updatedBooking == null)
            {
                _logger.LogWarning("Бронирование с id {Id} не найдено для обновления.", id);
                return NotFound($"Запись о бронировании с ID {id} не найдена.");
            }

            _logger.LogInformation("Обновлено бронирование с id {Id}.", id);
            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _bookingService.DeleteBookingAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Не удалось удалить бронирование с id {Id}. Не найдено.", id);
                return NotFound($"Запись о бронировании с ID {id} не найдена.");
            }

            _logger.LogInformation("Удалено бронирование с id {Id}.", id);
            return NoContent();
        }
    }
}
