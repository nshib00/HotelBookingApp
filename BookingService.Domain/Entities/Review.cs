using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
