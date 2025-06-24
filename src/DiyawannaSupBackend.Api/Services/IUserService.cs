using DiyawannaSupBackend.Api.Models.Entities;

namespace DiyawannaSupBackend.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(string id, User user);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<User>> SearchUsersByNameAsync(string name);
        Task<IEnumerable<User>> GetUsersByUniversityAsync(string university);
        Task<IEnumerable<User>> GetUsersByAgeRangeAsync(int minAge, int maxAge);
    }
}
