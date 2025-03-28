using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;

        public BookingRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task DeleteBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}
