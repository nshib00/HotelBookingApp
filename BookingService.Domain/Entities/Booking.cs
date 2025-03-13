using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("date_from")]
        public DateOnly DateFrom { get; set; }

        [Column("date_to")]
        public DateOnly DateTo { get; set; }

        [Column("price")]
        public double Price { get; set; }

        public User User { get; set; }
    }
}
