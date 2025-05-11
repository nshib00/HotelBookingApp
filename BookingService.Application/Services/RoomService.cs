using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookingApp.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IRoomRepository roomRepository, ILogger<RoomService> logger)
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RoomDTO>> GetHotelRoomsAsync(HotelDTO hotelDto)
        {
            _logger.LogInformation("Получение номеров для отеля с идентификатором {HotelId}", hotelDto.Id);
            var rooms = await _roomRepository.GetAllRoomsFromHotelAsync(hotelDto.Id);
            return rooms.ToDtoList();
        }

        public async Task<RoomDTO?> GetRoomByIdAsync(int id)
        {
            _logger.LogInformation("Получение номера с id={RoomId}", id);
            var room = await _roomRepository.GetRoomByIdAsync(id);
            return room?.ToDto();
        }

        public async Task<RoomDTO> AddRoomAsync(RoomDTO roomDto)
        {
            _logger.LogInformation("Добавление нового номера");

            var room = new Room
            {
                Name = roomDto.Name,
                Description = roomDto.Description,
                Price = roomDto.Price,
                Quantity = roomDto.Quantity,
                ImageUrl = roomDto.ImageUrl,
                Services = roomDto.Services,
            };

            var newRoom = await _roomRepository.AddRoomAsync(room);
            _logger.LogInformation("Номер создан с id={RoomId}", newRoom.Id);
            return newRoom.ToDto();
        }

        public async Task<RoomDTO?> UpdateRoomAsync(RoomDTO roomDto)
        {
            _logger.LogInformation("Обновление номера с id={RoomId}", roomDto.Id);

            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomDto.Id);
            if (existingRoom == null)
            {
                _logger.LogWarning("Номер с id={RoomId} не найден", roomDto.Id);
                return null;
            }

            existingRoom.Name = roomDto.Name;
            existingRoom.Description = roomDto.Description;
            existingRoom.Price = roomDto.Price;
            existingRoom.Quantity = roomDto.Quantity;
            existingRoom.ImageUrl = roomDto.ImageUrl;
            existingRoom.Services = roomDto.Services;

            var updatedRoom = await _roomRepository.UpdateRoomAsync(existingRoom);
            _logger.LogInformation("Номер с id={RoomId} обновлён", updatedRoom.Id);
            return updatedRoom.ToDto();
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            _logger.LogInformation("Удаление номера с id={RoomId}", id);

            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                _logger.LogWarning("Номер с id={RoomId} не найден", id);
                return false;
            }

            await _roomRepository.DeleteRoomAsync(room);
            _logger.LogInformation("Номер с id={RoomId} удалён", id);
            return true;
        }
    }
}
