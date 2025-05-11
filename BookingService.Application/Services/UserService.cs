using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace BookingApp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDTOPublic>> GetUsersAsync()
        {
            _logger.LogInformation("Получение списка всех пользователей");
            var users = await _userRepository.GetAllUsersAsync();
            return users.ToDtoList();
        }

        public async Task<UserDTOPublic?> GetUserByIdAsync(string id)
        {
            _logger.LogInformation("Получение пользователя с идентификатором {UserId}", id);
            var user = await _userRepository.GetUserByIdAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto)
        {
            _logger.LogInformation("Добавление нового пользователя с электронной почтой {UserEmail}", userDto.Email);

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                BirthDate = userDto.BirthDate,
                Email = userDto.Email,
            };

            var newUser = await _userRepository.AddUserAsync(user);
            _logger.LogInformation("Пользователь создан с идентификатором {UserId}", newUser.Id);
            return newUser.ToDto();
        }

        public async Task<UserDTO?> UpdateUserAsync(UserUpdateDTO userDto)
        {
            _logger.LogInformation("Обновление пользователя с идентификатором {UserId}", userDto.Id);

            var existingUser = await _userRepository.GetUserByIdAsync(userDto.Id);
            if (existingUser == null)
            {
                _logger.LogWarning("Пользователь с идентификатором {UserId} не найден", userDto.Id);
                return null;
            }

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.BirthDate = userDto.BirthDate;
            existingUser.Email = userDto.Email;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            _logger.LogInformation("Пользователь с идентификатором {UserId} обновлён", updatedUser.Id);
            return updatedUser.ToDto();
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            _logger.LogInformation("Удаление пользователя с идентификатором {UserId}", id);

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с идентификатором {UserId} не найден", id);
                return false;
            }

            await _userRepository.DeleteUserAsync(user);
            _logger.LogInformation("Пользователь с идентификатором {UserId} удалён", id);
            return true;
        }
    }
}
