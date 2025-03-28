using BookingApp.Domain.Entities;

namespace BookingApp.Application.DTOs
{
    public class UserDTOPublic
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }

    public class UserDTO : UserDTOPublic
    {
        public string HashedPassword { get; set; }
    }
}
