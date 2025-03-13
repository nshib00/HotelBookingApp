using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Domain.Entities
{
    [Table("hotels")]
    public class Hotel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("services_id")]
        public int ServicesId { get; set; }

        [Column("image_id")]
        public int ImageId { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
