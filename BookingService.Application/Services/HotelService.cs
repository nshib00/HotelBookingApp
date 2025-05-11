using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Extensions;

namespace BookingApp.Application.Services
{
    public class HotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<HotelDTO>> GetHotelsAsync()
        {
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
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            var dto = hotel?.ToDto();

            if (dto != null)
            {
                if (dto.Rooms.Any())
                    dto.MinRoomPrice = dto.Rooms.Min(r => r.Price);
            }

            return dto;
        }

        public async Task<HotelDTO> AddHotelAsync(HotelDTO hotelDto)
        {
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
            var existingHotel = await _hotelRepository.GetHotelByIdAsync(hotelDto.Id);
            if (existingHotel == null) return null;

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
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            if (hotel == null) return false;

            await _hotelRepository.DeleteHotelAsync(hotel);
            return true;
        }
    }
}
