using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly BookingDbContext _context;

        public HotelRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task<Hotel> AddHotelAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<Hotel> UpdateHotelAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task DeleteHotelAsync(Hotel hotel)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }
    }
}
