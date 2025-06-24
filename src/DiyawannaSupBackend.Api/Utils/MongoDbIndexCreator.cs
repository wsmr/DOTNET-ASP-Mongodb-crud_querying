using MongoDB.Driver;
using DiyawannaSupBackend.Api.Models.Entities; // Assuming your entities are here

namespace DiyawannaSupBackend.Api.Utils
{
    public static class MongoDbIndexCreator
    {
        public static async Task CreateIndexesAsync(IMongoDatabase database)
        {
            // User Collection Indexes
            var usersCollection = database.GetCollection<User>("users");
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Username), new CreateIndexOptions { Unique = true }));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Email), new CreateIndexOptions { Unique = true }));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Active)));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.University)));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Age)));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.CreatedAt)));
            await usersCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(u => u.Active),
                Builders<User>.IndexKeys.Ascending(u => u.University)
            )));

            // Add indexes for other collections (Universities, Faculties, Carts, Queries) similarly
            // Example for Universities:
            // var universitiesCollection = database.GetCollection<University>("universities");
            // await universitiesCollection.Indexes.CreateOneAsync(new CreateIndexModel<University>(Builders<University>.IndexKeys.Ascending(u => u.Name), new CreateIndexOptions { Unique = true }));
            // await universitiesCollection.Indexes.CreateOneAsync(new CreateIndexModel<University>(Builders<University>.IndexKeys.Ascending(u => u.Location)));
            // await universitiesCollection.Indexes.CreateOneAsync(new CreateIndexModel<University>(Builders<University>.IndexKeys.Ascending(u => u.Active)));
        }
    }
}
