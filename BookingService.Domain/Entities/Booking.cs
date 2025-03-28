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

        public string UserId { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        public User User { get; set; }
    }
}
