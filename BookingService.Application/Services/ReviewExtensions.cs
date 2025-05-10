using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class ReviewExtensions
    {
        public static ReviewDTO ToDto(this Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                HotelId = review.HotelId,
                UserId = review.UserId,
                CreatedAt = review.CreatedAt
            };
        }

        public static IEnumerable<ReviewDTO> ToDtoList(this IEnumerable<Review> reviews)
        {
            return reviews.Select(r => r.ToDto());
        }
    }
}
