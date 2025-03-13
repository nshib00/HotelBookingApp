using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Domain.Entities
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("hotel_id")]
        public int HotelId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("services_id")]
        public int ServicesId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("image_id")]
        public int ImageId { get; set; }

        public Hotel Hotel { get; set; }
    }
}
