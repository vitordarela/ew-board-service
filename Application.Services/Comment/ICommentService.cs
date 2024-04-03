using Domain.Model;
using Domain.Model.DTO.Comment;

namespace Application.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponse>> GetAllCommentsAsync(string taskId);

        Task<Comment> AddCommentAsync(string taskId, CommentRequest commentRequest);

        Task<CommentResponse> UpdateCommentAsync(string commentId, string taskId, CommentRequest commentRequest);

        Task DeleteCommentAsync(string userId, string commentId, string taskId);
    }
}
