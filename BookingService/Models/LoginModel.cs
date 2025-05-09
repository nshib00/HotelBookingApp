using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email обязателен")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
