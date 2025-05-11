using Moq;
using Xunit;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<ILogger<ReviewService>> _loggerMock;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _loggerMock = new Mock<ILogger<ReviewService>>();
        _reviewService = new ReviewService(_reviewRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetHotelReviewsAsync_ShouldReturnReviews()
    {
        var reviews = new List<Review>
        {
            new Review { Id = 1, Rating = 5, Comment = "Great Hotel!" }
        };
        _reviewRepositoryMock.Setup(repo => repo.GetAllHotelReviewsAsync(It.IsAny<int>())).ReturnsAsync(reviews);

        var result = await _reviewService.GetHotelReviewsAsync(1);

        Assert.Single(result);
        Assert.Equal(5, result.First().Rating);
    }

    [Fact]
    public async Task AddReviewAsync_ShouldReturnReview()
    {
        var reviewDto = new ReviewDTO { HotelId = 1, UserId = "user1", Rating = 5, Comment = "Great!" };
        var createdReview = new Review { Id = 1, HotelId = 1, UserId = "user1", Rating = 5, Comment = "Great!" };
        _reviewRepositoryMock.Setup(repo => repo.CreateReviewAsync(It.IsAny<Review>())).ReturnsAsync(createdReview);

        var result = await _reviewService.AddReviewAsync(reviewDto);

        Assert.NotNull(result);
        Assert.Equal("Great!", result.Comment);
    }
}
