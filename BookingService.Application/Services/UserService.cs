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

        public async Task<UserDTOPublic?> GetUserByIdAsync(string id)
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
                BirthDate = userDto.BirthDate,
                Email = userDto.Email,
            };

            var newUser = await _userRepository.AddUserAsync(user);
            return newUser.ToDto();
        }

        public async Task<UserDTO?> UpdateUserAsync(UserUpdateDTO userDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userDto.Id);
            if (existingUser == null) return null;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.BirthDate = userDto.BirthDate;
            existingUser.Email = userDto.Email;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return updatedUser.ToDto();
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteUserAsync(user);
            return true;
        }
    }
}
