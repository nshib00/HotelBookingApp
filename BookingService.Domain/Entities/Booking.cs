using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateOnly DateFrom { get; set; }

        [Required]
        public DateOnly DateTo { get; set; }

        [Required]
        public double Price { get; set; }

        public User User { get; set; }
    }
}
