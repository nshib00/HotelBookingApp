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
            return await _context.Hotels.Include(h => h.Rooms).Include(h => h.Services).ToListAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Services).Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
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

        public async Task<(IEnumerable<Hotel> Hotels, int TotalCount)> GetHotelsPagedAsync(int page, int pageSize)
        {
            var query = _context.Hotels
                                .Include(h => h.Rooms)
                                .Include(h => h.Services)
                                .AsQueryable();

            var totalCount = await query.CountAsync();

            var hotels = await query.Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            return (hotels, totalCount);
        }
    }
}
