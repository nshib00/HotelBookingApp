using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
