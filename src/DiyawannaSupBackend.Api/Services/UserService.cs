using DiyawannaSupBackend.Api.Exceptions;
using DiyawannaSupBackend.Api.Models.Entities;
using DiyawannaSupBackend.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace DiyawannaSupBackend.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        public UserService(IUserRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
            // This should ideally come from configuration
            _cacheDuration = TimeSpan.FromMinutes(5); 
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _cache.GetOrCreateAsync("AllUsers", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await _userRepository.GetAllAsync();
            });
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException($"User with username \'{user.Username}\' already exists.");
            }
            await _userRepository.CreateAsync(user);
            _cache.Remove("AllUsers"); // Invalidate cache
            return user;
        }

        public async Task UpdateUserAsync(string id, User userIn)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found.");
            }
            userIn.Id = existingUser.Id; // Ensure ID is consistent
            userIn.CreatedAt = existingUser.CreatedAt; // Preserve original creation date
            userIn.UpdatedAt = DateTime.UtcNow; // Update modified date
            await _userRepository.UpdateAsync(id, userIn);
            _cache.Remove("AllUsers"); // Invalidate cache
            _cache.Remove($"User_{id}"); // Invalidate specific user cache
        }

        public async Task DeleteUserAsync(string id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found.");
            }
            await _userRepository.DeleteAsync(id);
            _cache.Remove("AllUsers"); // Invalidate cache
            _cache.Remove($"User_{id}"); // Invalidate specific user cache
        }

        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string name)
        {
            return await _userRepository.SearchByNameAsync(name);
        }

        public async Task<IEnumerable<User>> GetUsersByUniversityAsync(string university)
        {
            return await _userRepository.GetByUniversityAsync(university);
        }

        public async Task<IEnumerable<User>> GetUsersByAgeRangeAsync(int minAge, int maxAge)
        {
            return await _userRepository.GetByAgeRangeAsync(minAge, maxAge);
        }
    }
}
