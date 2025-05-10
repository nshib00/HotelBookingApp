using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllHotelReviewsAsync(int hotelId);
        Task<IEnumerable<Review>> GetAllUserReviewsAsync(string userId);
        Task<Review?> GetReviewByIdAsync(int id);
        Task<Review> CreateReviewAsync(Review review);
        Task<Review> UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Review review);
    }
}
