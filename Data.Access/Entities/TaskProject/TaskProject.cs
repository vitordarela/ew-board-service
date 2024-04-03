using Domain.Model.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class TaskProject
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ProjectId { get; set; }

        public TaskProjectPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TaskProjectStatus Status { get; set; }
    }
}
