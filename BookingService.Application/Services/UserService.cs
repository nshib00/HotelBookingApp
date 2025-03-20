using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Extensions;

namespace BookingApp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTOPublic>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.ToDtoList();
        }

        public async Task<UserDTOPublic?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                BirthDate = userDto.BirthDate,
                Email = userDto.Email,
                IsAdmin = userDto.IsAdmin,
                Bookings = userDto.Bookings,
                HashedPassword = userDto.HashedPassword
            };

            var newUser = await _userRepository.AddUserAsync(user);
            return newUser.ToDto();
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO userDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userDto.Id);
            if (existingUser == null) return null;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.BirthDate = userDto.BirthDate;
            existingUser.Email = userDto.Email;
            existingUser.IsAdmin = userDto.IsAdmin;
            existingUser.Bookings = userDto.Bookings;
            existingUser.HashedPassword = userDto.HashedPassword;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return updatedUser.ToDto();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteUserAsync(user);
            return true;
        }
    }
}
