namespace BookingApp.Application.DTOs
{
    public class BookingCreateDTO
    {
        public int RoomId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class BookingDTO : BookingCreateDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int TotalDays { get; set; }
        public double TotalCost { get; set; }
    }
}
