using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email обязателен")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public required string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
