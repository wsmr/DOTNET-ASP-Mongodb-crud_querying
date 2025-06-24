using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiyawannaSupBackend.Api.Models.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store hashed password
        public int Age { get; set; }
        public string University { get; set; }
        public string School { get; set; }
        public string Work { get; set; }
        public bool Active { get; set; } = true;
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
