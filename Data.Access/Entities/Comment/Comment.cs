using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model.Entities.Comment
{
    public class Comment
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
