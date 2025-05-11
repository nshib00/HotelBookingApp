using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;

namespace BookingApp.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
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
                UserId = bookingDto.UserId,
                RoomId = bookingDto.RoomId ,
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
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(bookingDto.Id);
            if (existingBooking == null) return null;

            existingBooking.UserId = bookingDto.UserId;
            existingBooking.RoomId = bookingDto.RoomId;
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
