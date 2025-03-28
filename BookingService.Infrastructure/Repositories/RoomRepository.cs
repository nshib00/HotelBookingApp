using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BookingDbContext _context;

        public RoomRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task DeleteRoomAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsFromHotelAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.Include(r => r.Services).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }
    }
}
