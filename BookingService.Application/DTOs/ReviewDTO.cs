namespace BookingApp.Application.DTOs
{
    public class ReviewCreateDTO
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int HotelId { get; set; }
    }

    public class ReviewDTO : ReviewCreateDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
