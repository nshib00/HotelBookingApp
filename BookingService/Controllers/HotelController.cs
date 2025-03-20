﻿using BookingApp.Application.DTOs;
using BookingApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _hotelService;
        private readonly RoomService _roomService;

        public HotelController(HotelService hotelService, RoomService roomService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> Get()
        {
            var hotels = await _hotelService.GetHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDTO>> Get(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound($"Отель с ID {id} не найден.");
            }
            return Ok(hotel);
        }


        [HttpGet("{id}/Rooms")]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> GetRooms(int id)
        {
            HotelDTO? hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound($"Отель с ID {id} не найден.");
            }

            var hotelRooms = await _roomService.GetHotelRoomsAsync(hotel);
            return Ok(hotelRooms);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HotelDTO hotelDto)
        {
            if (hotelDto == null)
            {
                return BadRequest("Некорректные данные отеля.");
            }

            var createdHotel = await _hotelService.AddHotelAsync(hotelDto);
            return CreatedAtAction(nameof(Get), new { id = createdHotel.Id }, createdHotel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] HotelDTO hotelDto)
        {
            if (hotelDto == null || id != hotelDto.Id)
            {
                return BadRequest("Некорректные данные.");
            }

            var updatedHotel = await _hotelService.UpdateHotelAsync(hotelDto);
            if (updatedHotel == null)
            {
                return NotFound($"Отель с ID {id} не найден.");
            }

            return Ok(updatedHotel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _hotelService.DeleteHotelAsync(id);
            if (!deleted)
            {
                return NotFound($"Отель с ID {id} не найден.");
            }

            return NoContent();
        }
    }
}
