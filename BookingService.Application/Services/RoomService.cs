using BookingApp.Application.DTOs;
using BookingApp.Application.Extensions;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;

namespace BookingApp.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomDTO>> GetHotelRoomsAsync(HotelDTO hotelDto)
        {
            var rooms = await _roomRepository.GetAllRoomsFromHotelAsync(hotelDto.Id);
            return rooms.ToDtoList();
        }

        public async Task<RoomDTO?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            return room?.ToDto();
        }

        public async Task<RoomDTO> AddRoomAsync(RoomDTO roomDto)
        {
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
            return newRoom.ToDto();
        }

        public async Task<RoomDTO?> UpdateRoomAsync(RoomDTO roomDto)
        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomDto.Id);
            if (existingRoom == null) return null;

            existingRoom.Name = roomDto.Name;
            existingRoom.Description = roomDto.Description;
            existingRoom.Price = roomDto.Price;
            existingRoom.Quantity = roomDto.Quantity;
            existingRoom.ImageUrl = roomDto.ImageUrl;
            existingRoom.Services = roomDto.Services;

            var updatedRoom = await _roomRepository.UpdateRoomAsync(existingRoom);
            return updatedRoom.ToDto();
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null) return false;

            await _roomRepository.DeleteRoomAsync(room);
            return true;
        }
    }
}
