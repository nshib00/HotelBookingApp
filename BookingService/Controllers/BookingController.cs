using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;
        private readonly UserService _userService;

        public BookingController(BookingService bookingService, UserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> Get()
        {
            //var user = await _userService.GetUserByIdAsync(userId);
            //if (user == null)
            //    return NotFound($"Пользователь с id={userId} не найден.");

            //var bookings = await _bookingService.GetUserBookingsAsync(user);
            return Ok();
        }

        [HttpGet("{bookingId}")]
        public async Task<ActionResult<BookingDTO>> Get(int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Некорректные данные о бронировании.");
            }
            if (bookingDto.DateFrom > bookingDto.DateTo)
            {
                return BadRequest("Начало проживания не может быть позже конца проживания.");
            }

            var createdBooking = await _bookingService.AddBookingAsync(bookingDto);
            return CreatedAtAction(nameof(Get), new { id = createdBooking.Id }, createdBooking);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null || id != bookingDto.Id)
            {
                return BadRequest("Некорректные данные о бронировании.");
            }

            var updatedBooking = await _bookingService.UpdateBookingAsync(bookingDto);
            if (updatedBooking == null)
            {
                return NotFound($"Запись о бронировании с ID {id} не найдена.");
            }

            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _bookingService.DeleteBookingAsync(id);
            if (!deleted)
            {
                return NotFound($"Запись о бронировании с ID {id} не найдена.");
            }

            return NoContent();
        }

    }
}
