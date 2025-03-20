using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите Ваше имя.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Имя должно содержать от {2} до {1} символов.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите Вашу фамилию.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от {2} до {1} символов.")]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

