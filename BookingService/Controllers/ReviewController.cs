using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingApp.Api.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> Get(string userId)
        {
            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            _logger.LogInformation("Получен список всех отзывов пользователя id={userId}.", userId);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> Get(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                _logger.LogWarning("Отзыв с id {Id} не найден.", id);
                return NotFound($"Отзыв с id={id} не найден.");
            }

            _logger.LogInformation("Получен отзыв с id {Id}.", id);
            return Ok(review);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null)
            {
                _logger.LogWarning("Попытка создать отзыв с некорректными данными.");
                return BadRequest("Некорректные данные об отзыве.");
            }

            var createdReview = await _reviewService.AddReviewAsync(reviewDto);
            _logger.LogInformation("Создан отзыв с id {Id}.", createdReview.Id);
            return CreatedAtAction(nameof(Get), new { id = createdReview.Id }, createdReview);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null || id != reviewDto.Id)
            {
                _logger.LogWarning("Некорректные данные при обновлении отзыва id {Id}.", id);
                return BadRequest("Некорректные данные об отзыве.");
            }

            var updatedReview = await _reviewService.UpdateReviewAsync(reviewDto);
            if (updatedReview == null)
            {
                _logger.LogWarning("Отзыв с id {Id} не найден для обновления.", id);
                return NotFound($"Отзыв с id={id} не найден.");
            }

            _logger.LogInformation("Обновлен отзыв с id {Id}.", id);
            return Ok(updatedReview);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _reviewService.DeleteReviewAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Не удалось удалить отзыв с id {Id}. Не найден.", id);
                return NotFound($"Отзыв с id={id} не найден.");
            }

            _logger.LogInformation("Удален отзыв с id {Id}.", id);
            return NoContent();
        }
    }
}
