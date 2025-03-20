using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("hotel_services")]
    public class HotelService
    {
        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
