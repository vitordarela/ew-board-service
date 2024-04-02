using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class Project
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
