namespace Domain.Model.Common
{
    public abstract class BaseModelEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
