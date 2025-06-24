using DiyawannaSupBackend.Api.Models.Entities;

namespace DiyawannaSupBackend.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User user);
        Task DeleteAsync(string id); // Soft delete
        Task<IEnumerable<User>> SearchByNameAsync(string name);
        Task<IEnumerable<User>> GetByUniversityAsync(string university);
        Task<IEnumerable<User>> GetByAgeRangeAsync(int minAge, int maxAge);
    }
}
