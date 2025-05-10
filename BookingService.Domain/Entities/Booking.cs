using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Domain.Entities
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public string? UserId { get; set; }

        public required DateTime DateFrom { get; set; }

        public required DateTime DateTo { get; set; }

        public User User { get; set; }

        [NotMapped]
        public int TotalDays => (DateTo - DateFrom).Days;

        [NotMapped]
        public double TotalCost => TotalDays * Room.Price;
    }
}
