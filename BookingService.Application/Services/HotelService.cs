using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace BookingApp.Application.Services
{
    public class HotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<HotelService> _logger;

        public HotelService(IHotelRepository hotelRepository, ILogger<HotelService> logger)
        {
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<HotelDTO>> GetHotelsAsync()
        {
            _logger.LogInformation("Получение списка всех отелей");
            var hotels = await _hotelRepository.GetAllHotelsAsync();
            var hotelDtos = hotels.ToDtoList();

            foreach (var dto in hotelDtos)
            {
                if (dto.Rooms != null && dto.Rooms.Any())
                    dto.MinRoomPrice = dto.Rooms.Min(r => r.Price);
            }

            return hotelDtos;
        }

        public async Task<HotelDTO?> GetHotelByIdAsync(int id)
        {
            _logger.LogInformation("Получение отеля с Id: {HotelId}", id);
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            var dto = hotel?.ToDto();

            if (dto != null && dto.Rooms.Any())
                dto.MinRoomPrice = dto.Rooms.Min(r => r.Price);

            return dto;
        }

        public async Task<HotelDTO> AddHotelAsync(HotelDTO hotelDto)
        {
            _logger.LogInformation("Добавление нового отеля: {HotelName}", hotelDto.Name);
            var hotel = new Hotel
            {
                Name = hotelDto.Name,
                Rooms = hotelDto.Rooms,
                Description = hotelDto.Description,
                City = hotelDto.City,
                Address = hotelDto.Address,
                StarRating = hotelDto.StarRating,
                Services = hotelDto.Services,
                ImageUrl = hotelDto.ImageUrl
            };

            var newHotel = await _hotelRepository.AddHotelAsync(hotel);
            hotelDto.Id = newHotel.Id;

            if (hotelDto.Rooms.Any())
                hotelDto.MinRoomPrice = hotelDto.Rooms.Min(r => r.Price);

            return hotelDto;
        }

        public async Task<HotelDTO?> UpdateHotelAsync(HotelUpdateDTO hotelDto)
        {
            _logger.LogInformation("Обновление отеля с Id: {HotelId}", hotelDto.Id);
            var existingHotel = await _hotelRepository.GetHotelByIdAsync(hotelDto.Id);
            if (existingHotel == null)
            {
                _logger.LogWarning("Отель с Id {HotelId} не найден", hotelDto.Id);
                return null;
            }

            existingHotel.Name = hotelDto.Name;
            existingHotel.Description = hotelDto.Description;
            existingHotel.City = hotelDto.City;
            existingHotel.Address = hotelDto.Address;
            existingHotel.StarRating = hotelDto.StarRating;
            existingHotel.ImageUrl = hotelDto.ImageUrl ?? "";

            var updatedHotel = await _hotelRepository.UpdateHotelAsync(existingHotel);
            var updatedDto = updatedHotel.ToDto();

            if (updatedDto.Rooms.Any())
                updatedDto.MinRoomPrice = updatedDto.Rooms.Min(r => r.Price);

            return updatedDto;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            _logger.LogInformation("Удаление отеля с Id: {HotelId}", id);
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                _logger.LogWarning("Отель с Id {HotelId} не найден", id);
                return false;
            }

            await _hotelRepository.DeleteHotelAsync(hotel);
            return true;
        }

        public async Task<PagedResult<HotelDTO>> GetHotelsPagedAsync(int page, int pageSize)
        {
            _logger.LogInformation("Получение отелей постранично: страница {Page}, размер {PageSize}", page, pageSize);
            var (hotels, totalCount) = await _hotelRepository.GetHotelsPagedAsync(page, pageSize);
            var hotelDtos = hotels.ToDtoList();

            foreach (var dto in hotelDtos)
            {
                if (dto.Rooms != null && dto.Rooms.Any())
                    dto.MinRoomPrice = dto.Rooms.Min(r => r.Price);
            }

            return new PagedResult<HotelDTO>
            {
                Items = hotelDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
