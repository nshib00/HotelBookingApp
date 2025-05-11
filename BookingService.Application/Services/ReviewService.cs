using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookingApp.Application.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewDTO>> GetHotelReviewsAsync(int hotelId)
        {
            _logger.LogInformation("Получение отзывов для отеля с id={HotelId}", hotelId);
            var reviews = await _reviewRepository.GetAllHotelReviewsAsync(hotelId);
            return reviews.ToDtoList();
        }

        public async Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(string userId)
        {
            _logger.LogInformation("Получение отзывов пользователя с id={UserId}", userId);
            var reviews = await _reviewRepository.GetAllUserReviewsAsync(userId);
            return reviews.ToDtoList();
        }

        public async Task<ReviewDTO?> GetReviewByIdAsync(int id)
        {
            _logger.LogInformation("Получение отзыва с id={ReviewId}", id);
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            return review?.ToDto();
        }

        public async Task<ReviewDTO> AddReviewAsync(ReviewDTO reviewDto)
        {
            _logger.LogInformation("Добавление нового отзыва для отеля c id={HotelId} от пользователя с id={UserId}", reviewDto.HotelId, reviewDto.UserId);

            var review = new Review
            {
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                HotelId = reviewDto.HotelId,
                UserId = reviewDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var newReview = await _reviewRepository.CreateReviewAsync(review);
            _logger.LogInformation("Отзыв создан с id={ReviewId}", newReview.Id);
            return newReview.ToDto();
        }

        public async Task<ReviewDTO?> UpdateReviewAsync(ReviewDTO reviewDto)
        {
            _logger.LogInformation("Обновление отзыва с id={ReviewId}", reviewDto.Id);

            var existingReview = await _reviewRepository.GetReviewByIdAsync(reviewDto.Id);
            if (existingReview == null)
            {
                _logger.LogWarning("Отзыв с id={ReviewId} не найден", reviewDto.Id);
                return null;
            }

            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Comment;
            existingReview.HotelId = reviewDto.HotelId;

            var updatedReview = await _reviewRepository.UpdateReviewAsync(existingReview);
            _logger.LogInformation("Отзыв с id={ReviewId} обновлён", updatedReview.Id);
            return updatedReview.ToDto();
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            _logger.LogInformation("Удаление отзыва с id={ReviewId}", id);

            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                _logger.LogWarning("Отзыв с id={ReviewId} не найден", id);
                return false;
            }

            await _reviewRepository.DeleteReviewAsync(review);
            _logger.LogInformation("Отзыв с id={ReviewId} удалён", id);
            return true;
        }
    }
}
