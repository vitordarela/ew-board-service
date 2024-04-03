using Domain.Model;

namespace Application.Services.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync(string taskId);

        Task<Comment> AddAsync(Comment comment);

        Task<Comment> UpdateAsync(Comment comment);

        Task DeleteAsync(string userId, string commentId, string taskId);
    }
}
