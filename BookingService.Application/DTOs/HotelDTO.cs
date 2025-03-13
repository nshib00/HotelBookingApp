using BookingApp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Application.DTOs
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
