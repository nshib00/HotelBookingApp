using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class BookingExtensions
    {
        public static BookingDTO ToDto(this Booking booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                Room = booking.Room,
                User = booking.User,
                DateFrom = booking.DateFrom,
                DateTo = booking.DateTo,
            };
        }

        public static IEnumerable<BookingDTO> ToDtoList(this IEnumerable<Booking> bookings)
        {
            return bookings.Select(b => b.ToDto());
        }
    }
}