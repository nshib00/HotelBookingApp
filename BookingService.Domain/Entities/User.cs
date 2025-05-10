using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookingApp.Domain.Entities
{
    public class User : IdentityUser
    {            
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Имя должно содержать от {2} до {1} символов.")]
        public string FirstName { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от {2} до {1} символов.")]
        public string LastName { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

