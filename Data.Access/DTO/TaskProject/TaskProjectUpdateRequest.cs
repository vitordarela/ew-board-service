namespace Domain.Model.DTO.TaskBoard
{
    public class TaskProjectUpdateRequest
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}
