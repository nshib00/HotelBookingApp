using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;

namespace BookingApp.Application.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewDTO>> GetHotelReviewsAsync(int hotelId)
        {
            var reviews = await _reviewRepository.GetAllHotelReviewsAsync(hotelId);
            return reviews.ToDtoList();
        }

        public async Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(string userId)
        {
            var reviews = await _reviewRepository.GetAllUserReviewsAsync(userId);
            return reviews.ToDtoList();
        }

        public async Task<ReviewDTO?> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            return review?.ToDto();
        }

        public async Task<ReviewDTO> AddReviewAsync(ReviewDTO reviewDto)
        {
            var review = new Review
            {
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                HotelId = reviewDto.HotelId,
                UserId = reviewDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var newReview = await _reviewRepository.CreateReviewAsync(review);
            return newReview.ToDto();
        }

        public async Task<ReviewDTO?> UpdateReviewAsync(ReviewDTO reviewDto)
        {
            var existingReview = await _reviewRepository.GetReviewByIdAsync(reviewDto.Id);
            if (existingReview == null) return null;

            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Comment;
            existingReview.HotelId = reviewDto.HotelId;

            var updatedReview = await _reviewRepository.UpdateReviewAsync(existingReview);
            return updatedReview.ToDto();
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return false;

            await _reviewRepository.DeleteReviewAsync(review);
            return true;
        }
    }
}
