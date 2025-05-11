using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookingApp.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IHotelRepository hotelRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingDTO?>> GetUserBookingsAsync(UserDTOPublic userDto)
        {
            _logger.LogInformation("Получение всех бронирований пользователя с Id: {UserId}", userDto.Id);
            var bookings = await _bookingRepository.GetAllUserBookingsAsync(userDto.Id);
            return bookings.ToDtoList();
        }

        public async Task<BookingDTO?> GetBookingByIdAsync(int id)
        {
            _logger.LogInformation("Получение бронирования с Id: {BookingId}", id);
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            return booking?.ToDto();
        }

        public async Task<BookingDTO> AddBookingAsync(BookingDTO bookingDto)
        {
            _logger.LogInformation("Добавление нового бронирования для пользователя {UserId} и комнаты {RoomId}", bookingDto.UserId, bookingDto.RoomId);
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                RoomId = bookingDto.RoomId,
                DateFrom = bookingDto.DateFrom,
                DateTo = bookingDto.DateTo,
            };

            var newBooking = await _bookingRepository.CreateBookingAsync(booking);
            newBooking.Room = await _roomRepository.GetRoomByIdAsync(newBooking.RoomId);
            newBooking.Hotel = await _hotelRepository.GetHotelByIdAsync(newBooking.Room.HotelId);

            return newBooking.ToDto();
        }

        public async Task<BookingDTO?> UpdateBookingAsync(BookingDTO bookingDto)
        {
            _logger.LogInformation("Обновление бронирования с id: {BookingId}", bookingDto.Id);
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(bookingDto.Id);
            if (existingBooking == null)
            {
                _logger.LogWarning("Бронирование с id {BookingId} не найдено", bookingDto.Id);
                return null;
            }

            existingBooking.UserId = bookingDto.UserId;
            existingBooking.RoomId = bookingDto.RoomId;
            existingBooking.DateFrom = bookingDto.DateFrom;
            existingBooking.DateTo = bookingDto.DateTo;

            var updatedBooking = await _bookingRepository.UpdateBookingAsync(existingBooking);
            return updatedBooking.ToDto();
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            _logger.LogInformation("Удаление бронирования с id: {BookingId}", id);
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning("Бронирование с id {BookingId} не найдено", id);
                return false;
            }

            await _bookingRepository.DeleteBookingAsync(booking);
            return true;
        }
    }
}
