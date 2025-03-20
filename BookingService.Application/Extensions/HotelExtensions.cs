using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class HotelExtensions
    {
        public static HotelDTO ToDto(this Hotel hotel)
        {
            return new HotelDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rooms = hotel.Rooms,
                Description = hotel.Description,
                City = hotel.City,
                Address = hotel.Address,
                Rating = hotel.Rating,
                Services = hotel.Services,
                ImageUrl = hotel.ImageUrl
            };
        }

        public static IEnumerable<HotelDTO> ToDtoList(this IEnumerable<Hotel> hotels)
        {
            return hotels.Select(h => h.ToDto());
        }
    }
}
