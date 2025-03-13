using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [Required]
        public string FirstName { get; set; }

        [Column("email")]
        [Required]
        public string LastName { get; set; }

        [Column("email")]
        public string PhoneNumber { get; set; }

        [Column("email")]
        [Required]
        public DateOnly BirthDate { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        [Column("hashed_password")]
        [Required]
        public string HashedPassword { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

