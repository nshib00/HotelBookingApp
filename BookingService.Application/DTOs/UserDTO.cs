using BookingApp.Domain.Entities;

namespace BookingApp.Application.DTOs
{
    public class UserUpdateDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UserDTOPublic : UserUpdateDTO
    {
        public ICollection<Booking> Bookings { get; set; }
    }

    public class UserDTO : UserDTOPublic
    {
        public string HashedPassword { get; set; }
    }
}
