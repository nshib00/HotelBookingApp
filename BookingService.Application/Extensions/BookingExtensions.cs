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
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                DateFrom = booking.DateFrom,
                DateTo = booking.DateTo,
                TotalDays = booking.TotalDays,
                TotalCost = booking.TotalCost
            };
        }

        public static IEnumerable<BookingDTO> ToDtoList(this IEnumerable<Booking> bookings)
        {
            return bookings.Select(b => b.ToDto());
        }
    }
}