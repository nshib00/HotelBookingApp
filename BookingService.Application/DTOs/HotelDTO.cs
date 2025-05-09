using BookingApp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Application.DTOs
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [Range(1, 5)]
        public int StarRating { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public ICollection<HotelService> Services { get; set; }

        public double? MinRoomPrice { get; set; }
    }
}
