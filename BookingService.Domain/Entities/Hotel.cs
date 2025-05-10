using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Domain.Entities
{
    [Table("hotels")]
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [Range(1, 5)]
        public int StarRating { get; set; }

        public int ServicesId { get; set; }

        [Url]
        public string ImageUrl { get; set; } = "";

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public ICollection<HotelService> Services { get; set; } = new List<HotelService>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
