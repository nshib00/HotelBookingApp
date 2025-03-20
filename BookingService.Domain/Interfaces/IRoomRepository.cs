using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRoomsFromHotelAsync(int hotelId);
        Task<Room?> GetRoomByIdAsync(int id);
        Task<Room> AddRoomAsync(Room room);
        Task<Room> UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(Room room);
    }
}
