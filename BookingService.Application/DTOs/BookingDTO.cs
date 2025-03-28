using BookingApp.Domain.Entities;
namespace BookingApp.Application.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public Room Room { get; set; }

        public User User { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public double Price { get; set; }
    }
}
