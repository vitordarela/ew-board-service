using Domain.Model.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class User
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
