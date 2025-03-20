using BookingApp.Domain.Entities;
namespace BookingApp.Application.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public Room Room { get; set; }

        public User User { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public double Price { get; set; }
    }
}
