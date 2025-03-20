using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("room_services")]
    public class RoomService
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } = 0; // если Price=0, услуга включена в стоимость номера
    }
}
