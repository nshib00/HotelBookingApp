using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel> AddHotelAsync(Hotel hotel);
        Task<Hotel> UpdateHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(Hotel hotel);
        Task<(IEnumerable<Hotel> Hotels, int TotalCount)> GetHotelsPagedAsync(int page, int pageSize);
    }
}
