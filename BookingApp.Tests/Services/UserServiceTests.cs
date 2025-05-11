using Moq;
using Xunit;
using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BookingApp.Domain.Entities;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((User)null);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnCreatedUser()
    {
        var userDto = new UserDTO { UserName = "newuser", Email = "newuser@example.com" };
        var user = new BookingApp.Domain.Entities.User { Id = 1, UserName = "newuser", Email = "newuser@example.com" };
        _mockUserRepository.Setup(r => r.CreateUserAsync(It.IsAny<BookingApp.Domain.Entities.User>())).ReturnsAsync(user);

        var result = await _userService.CreateUserAsync(userDto);

        Assert.NotNull(result);
        Assert.Equal("newuser", result.UserName);
        Assert.Equal("newuser@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUpdatedUser()
    {
        var userDto = new UserDTO { Id = 1, UserName = "updateduser", Email = "updated@example.com" };
        var user = new User { Id = 1, UserName = "updateduser", Email = "updated@example.com" };
        _mockUserRepository.Setup(r => r.UpdateUserAsync(It.IsAny<BookingApp.Domain.Entities.User>())).ReturnsAsync(user);

        var result = await _userService.UpdateUserAsync(userDto);

        Assert.NotNull(result);
        Assert.Equal("updateduser", result.UserName);
        Assert.Equal("updated@example.com", result.Email);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
    {
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockUserRepository.Setup(r => r.DeleteUserAsync(user)).Returns(Task.CompletedTask);

        var result = await _userService.DeleteUserAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((User)null);

        var result = await _userService.DeleteUserAsync(1);

        Assert.False(result);
    }
}
