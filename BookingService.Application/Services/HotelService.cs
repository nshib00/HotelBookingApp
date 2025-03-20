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
            return hotels.ToDtoList();
        }

        public async Task<HotelDTO?> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            return hotel?.ToDto();
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
                Rating = hotelDto.Rating,
                Services = hotelDto.Services,
                ImageUrl = hotelDto.ImageUrl
            };

            var newHotel = await _hotelRepository.AddHotelAsync(hotel);
            return newHotel.ToDto();
        }

        public async Task<HotelDTO?> UpdateHotelAsync(HotelDTO hotelDto)
        {
            var existingHotel = await _hotelRepository.GetHotelByIdAsync(hotelDto.Id);
            if (existingHotel == null) return null;

            existingHotel.Name = hotelDto.Name;
            existingHotel.Rooms = hotelDto.Rooms;

            var updatedHotel = await _hotelRepository.UpdateHotelAsync(existingHotel);
            return updatedHotel.ToDto();
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
