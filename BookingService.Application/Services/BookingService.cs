using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;

namespace BookingApp.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingDTO?>> GetUserBookingsAsync(UserDTOPublic userDto)
        {
            var bookings = await _bookingRepository.GetAllUserBookingsAsync(userDto.Id);
            return bookings.ToDtoList();
        }

        public async Task<BookingDTO?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            return booking?.ToDto();
        }

        public async Task<BookingDTO> AddBookingAsync(BookingDTO bookingDto)
        {
            var booking = new Booking
            {
                User = bookingDto.User,
                Room = bookingDto.Room,
                DateFrom = bookingDto.DateFrom,
                DateTo = bookingDto.DateTo,
            };

            var newBooking = await _bookingRepository.CreateBookingAsync(booking);
            return newBooking.ToDto();
        }

        public async Task<BookingDTO?> UpdateBookingAsync(BookingDTO bookingDto)
        {
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(bookingDto.Id);
            if (existingBooking == null) return null;

            existingBooking.User = bookingDto.User;
            existingBooking.Room = bookingDto.Room;
            existingBooking.DateFrom = bookingDto.DateFrom;
            existingBooking.DateTo = bookingDto.DateTo;

            var updatedBooking = await _bookingRepository.UpdateBookingAsync(existingBooking);
            return updatedBooking.ToDto();
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return false;

            await _bookingRepository.DeleteBookingAsync(booking);
            return true;
        }
    }
}
