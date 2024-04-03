using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class Comment
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string TaskId { get; set; }

        public string UserId { get; set; }
    }
}
