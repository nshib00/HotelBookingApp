using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<ILogger<BookingApp.Application.Services.HotelService>> _loggerMock;
    private readonly BookingApp.Application.Services.HotelService _hotelService;

    public HotelServiceTests()
    {
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        _loggerMock = new Mock<ILogger<BookingApp.Application.Services.HotelService>>();
        _hotelService = new BookingApp.Application.Services.HotelService(_hotelRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetHotelsAsync_ShouldReturnHotels()
    {
        var hotels = new List<Hotel>
        {
            new Hotel { Id = 1, Name = "Hotel 1", Rooms = new List<Room> { new Room { Price = 100 } } }
        };
        _hotelRepositoryMock.Setup(repo => repo.GetAllHotelsAsync()).ReturnsAsync(hotels);

        var result = await _hotelService.GetHotelsAsync();

        Assert.Single(result);
        Assert.Equal("Hotel 1", result.First().Name);
    }

    [Fact]
    public async Task AddHotelAsync_ShouldReturnHotel()
    {
        var hotelDto = new HotelDTO { Name = "Hotel 2" };
        var createdHotel = new Hotel { Id = 2, Name = "Hotel 2" };
        _hotelRepositoryMock.Setup(repo => repo.AddHotelAsync(It.IsAny<Hotel>())).ReturnsAsync(createdHotel);

        var result = await _hotelService.AddHotelAsync(hotelDto);

        Assert.NotNull(result);
        Assert.Equal("Hotel 2", result.Name);
    }

    [Fact]
    public async Task DeleteHotelAsync_ShouldReturnTrue_WhenHotelExists()
    {
        var hotelId = 1;
        var hotel = new Hotel { Id = hotelId };
        _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(hotelId)).ReturnsAsync(hotel);
        _hotelRepositoryMock.Setup(repo => repo.DeleteHotelAsync(It.IsAny<Hotel>())).Returns(Task.CompletedTask);

        var result = await _hotelService.DeleteHotelAsync(hotelId);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteHotelAsync_ShouldReturnFalse_WhenHotelDoesNotExist()
    {
        var hotelId = 1;
        _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

        var result = await _hotelService.DeleteHotelAsync(hotelId);

        Assert.False(result);
    }
}
