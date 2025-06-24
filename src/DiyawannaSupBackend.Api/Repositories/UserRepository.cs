using DiyawannaSupBackend.Api.Models.Entities;
using MongoDB.Driver;

namespace DiyawannaSupBackend.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<User> GetByIdAsync(string id) =>
            await _users.Find(user => user.Id == id && user.Active).FirstOrDefaultAsync();

        public async Task<User> GetByUsernameAsync(string username) =>
            await _users.Find(user => user.Username == username && user.Active).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _users.Find(user => user.Active).ToListAsync();

        public async Task CreateAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task DeleteAsync(string id) =>
            await _users.UpdateOneAsync(user => user.Id == id, Builders<User>.Update.Set(u => u.Active, false));

        public async Task<IEnumerable<User>> SearchByNameAsync(string name) =>
            await _users.Find(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && user.Active).ToListAsync();

        public async Task<IEnumerable<User>> GetByUniversityAsync(string university) =>
            await _users.Find(user => user.University == university && user.Active).ToListAsync();

        public async Task<IEnumerable<User>> GetByAgeRangeAsync(int minAge, int maxAge) =>
            await _users.Find(user => user.Age >= minAge && user.Age <= maxAge && user.Active).ToListAsync();
    }
}
