using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class HotelExtensions
    {
        public static HotelDTO ToDto(this Hotel hotel)
        {
            var dto = new HotelDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rooms = hotel.Rooms,
                Description = hotel.Description,
                City = hotel.City,
                Address = hotel.Address,
                StarRating = hotel.StarRating,
                Services = hotel.Services,
                ImageUrl = hotel.ImageUrl
            };

            if (hotel.Rooms != null && hotel.Rooms.Any())
            {
                dto.MinRoomPrice = hotel.Rooms.Min(r => r.Price);
            }

            return dto;
        }
        public static IEnumerable<HotelDTO> ToDtoList(this IEnumerable<Hotel> hotels)
        {
            return hotels.Select(h => h.ToDto());
        }
    }
}
