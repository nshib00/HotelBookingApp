using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int HotelId { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int ServicesId { get; set; }

        public int Quantity { get; set; }

        [Url]
        public string ImageUrl { get; set; } = "";

        public ICollection<RoomService> Services { get; set; } = new List<RoomService>();
    }
}
