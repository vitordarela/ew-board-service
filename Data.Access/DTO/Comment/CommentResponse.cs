namespace Domain.Model.DTO.Comment
{
    public class CommentResponse
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
    }
}
