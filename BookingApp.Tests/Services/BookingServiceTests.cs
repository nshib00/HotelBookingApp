using Moq;
using Xunit;
using BookingApp.Application.Services;
using BookingApp.Application.DTOs;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApp.Application.Tests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<ILogger<BookingService>> _loggerMock;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _loggerMock = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(_bookingRepositoryMock.Object, _roomRepositoryMock.Object, _hotelRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUserBookingsAsync_ShouldReturnBookings()
        {
            // Arrange
            var userDto = new UserDTOPublic { Id = 1 };
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, UserId = 1, RoomId = 1, DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(2) }
            };
            _bookingRepositoryMock.Setup(repo => repo.GetAllUserBookingsAsync(userDto.Id)).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetUserBookingsAsync(userDto);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task AddBookingAsync_ShouldReturnBooking()
        {
            // Arrange
            var bookingDto = new BookingDTO
            {
                UserId = 1,
                RoomId = 1,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(2)
            };

            var createdBooking = new Booking
            {
                Id = 1,
                UserId = 1,
                RoomId = 1,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(2)
            };

            _bookingRepositoryMock.Setup(repo => repo.CreateBookingAsync(It.IsAny<Booking>())).ReturnsAsync(createdBooking);
            _roomRepositoryMock.Setup(repo => repo.GetRoomByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { HotelId = 1 });
            _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(It.IsAny<int>())).ReturnsAsync(new Hotel());

            // Act
            var result = await _bookingService.AddBookingAsync(bookingDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeleteBookingAsync_ShouldReturnTrue_WhenBookingExists()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { Id = bookingId };
            _bookingRepositoryMock.Setup(repo => repo.GetBookingByIdAsync(bookingId)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(repo => repo.DeleteBookingAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);

            // Act
            var result = await _bookingService.DeleteBookingAsync(bookingId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteBookingAsync_ShouldReturnFalse_WhenBookingDoesNotExist()
        {
            // Arrange
            var bookingId = 1;
            _bookingRepositoryMock.Setup(repo => repo.GetBookingByIdAsync(bookingId)).ReturnsAsync((Booking)null);

            // Act
            var result = await _bookingService.DeleteBookingAsync(bookingId);

            // Assert
            Assert.False(result);
        }
    }
}
