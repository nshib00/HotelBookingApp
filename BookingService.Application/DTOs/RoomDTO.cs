using BookingApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Application.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HotelDTO? Hotel { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public ICollection<RoomService> Services;
    }
}
