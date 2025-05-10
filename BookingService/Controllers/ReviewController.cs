using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingApp.Api.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;

        public ReviewController(ReviewService reviewService, UserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetHotelReviews(int hotelId)
        {
            var reviews = await _reviewService.GetHotelReviewsAsync(hotelId);
            return Ok(reviews);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetMyReviews()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Пользователь не найден.");

            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        public async Task<ActionResult<ReviewDTO>> Get(int reviewId)
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReviewCreateDTO reviewDto)
        {
            if (reviewDto == null || reviewDto.Rating < 1 || reviewDto.Rating > 5)
                return BadRequest("Некорректные данные отзыва.");

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Пользователь не найден.");

            var newReviewDto = new ReviewDTO
            {
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                HotelId = reviewDto.HotelId,
                UserId = userId
            };

            var createdReview = await _reviewService.AddReviewAsync(newReviewDto);
            return CreatedAtAction(nameof(Get), new { reviewId = createdReview.Id }, createdReview);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null || id != reviewDto.Id)
                return BadRequest("Некорректные данные отзыва.");

            var updatedReview = await _reviewService.UpdateReviewAsync(reviewDto);
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _reviewService.DeleteReviewAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
