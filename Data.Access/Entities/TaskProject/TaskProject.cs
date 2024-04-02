using Domain.Model.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Model
{
    public class TaskProject
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public string Name { get; set; }

        public string ProjectId { get; set; }

        public TaskProjectPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public TaskProjectStatus Status { get; set; }
    }
}
