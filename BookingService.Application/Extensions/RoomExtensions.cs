using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class RoomExtensions
    {
        public static RoomDTO ToDto(this Room room)
        {
            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Price = room.Price,
                Quantity = room.Quantity,
                ImageUrl = room.ImageUrl,
                Services = room.Services,
            };
        }

        public static IEnumerable<RoomDTO> ToDtoList(this IEnumerable<Room> rooms)
        {
            return rooms.Select(r => r.ToDto());
        }
    }
}
