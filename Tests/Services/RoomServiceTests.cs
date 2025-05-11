using Moq;
using Xunit;
using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly Mock<ILogger<RoomService>> _mockLogger;
    private readonly RoomService _roomService;

    public RoomServiceTests()
    {
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockLogger = new Mock<ILogger<RoomService>>();
        _roomService = new RoomService(_mockRoomRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetHotelRoomsAsync_ShouldReturnRoomList()
    {
        var rooms = new List<BookingApp.Domain.Entities.Room>
        {
            new BookingApp.Domain.Entities.Room { Id = 1, Name = "Room A" },
            new BookingApp.Domain.Entities.Room { Id = 2, Name = "Room B" }
        };
        _mockRoomRepository.Setup(r => r.GetAllRoomsFromHotelAsync(1)).ReturnsAsync(rooms);

        var result = await _roomService.GetHotelRoomsAsync(new HotelDTO { Id = 1 });

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddRoomAsync_ShouldReturnAddedRoom()
    {
        var roomDto = new RoomDTO { Name = "Room A", Price = 100 };
        var room = new BookingApp.Domain.Entities.Room { Id = 1, Name = "Room A", Price = 100 };
        _mockRoomRepository.Setup(r => r.AddRoomAsync(It.IsAny<BookingApp.Domain.Entities.Room>())).ReturnsAsync(room);

        var result = await _roomService.AddRoomAsync(roomDto);

        Assert.NotNull(result);
        Assert.Equal("Room A", result.Name);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldReturnTrue_WhenRoomExists()
    {
        var room = new BookingApp.Domain.Entities.Room { Id = 1, Name = "Room A" };
        _mockRoomRepository.Setup(r => r.GetRoomByIdAsync(1)).ReturnsAsync(room);
        _mockRoomRepository.Setup(r => r.DeleteRoomAsync(room)).Returns(Task.CompletedTask);

        var result = await _roomService.DeleteRoomAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldReturnFalse_WhenRoomNotFound()
    {
        _mockRoomRepository.Setup(r => r.GetRoomByIdAsync(1)).ReturnsAsync((BookingApp.Domain.Entities.Room)null);

        var result = await _roomService.DeleteRoomAsync(1);

        Assert.False(result);
    }
}
