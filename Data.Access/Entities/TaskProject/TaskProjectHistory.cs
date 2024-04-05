using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class TaskProjectHistory
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CollectionName { get; set; }

        public string Action { get; set; }

        public string NewValues { get; set; }

        public DateTime DateTime { get; set; }

        public string UserId { get; set; }

        public string TaskId { get; set; }
    }
}