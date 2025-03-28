using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookingApp.Domain.Entities
{
    public class User : IdentityUser
    {            
        [Required(ErrorMessage = "Укажите Ваше имя.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Имя должно содержать от {2} до {1} символов.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите Вашу фамилию.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от {2} до {1} символов.")]
        public string LastName { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

