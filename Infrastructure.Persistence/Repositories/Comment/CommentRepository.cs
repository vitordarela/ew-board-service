using Application.Services.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbSet<Comment> _comment;
        private readonly MongoDbContext dbContext;

        public CommentRepository(MongoDbContext dbContext)
        {
            _comment = dbContext.Comment;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(string taskId)
        {
            return await _comment.Where(p => p.TaskId == taskId).ToListAsync();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var commentSaved = await _comment.AddAsync(comment);
            dbContext.SaveChanges();

            return commentSaved.Entity;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var existingComment = await _comment.FirstOrDefaultAsync(t => t.Id == comment.Id & t.TaskId == comment.TaskId & t.UserId == comment.UserId );

            if (existingComment != null)
            {
                existingComment.Description = comment.Description;
                existingComment.UserId = comment.UserId;
                dbContext.SaveChanges();
            }

            return existingComment;
        }

        public async Task DeleteAsync(string userId, string commentId, string taskId)
        {
            var commentSearch = await _comment.FirstOrDefaultAsync(p => p.Id == commentId & p.UserId == userId & p.TaskId == taskId);

            if(commentSearch != null)
            {
                _comment.Remove(commentSearch);
                dbContext.SaveChanges();
            }  
        }
    }
}
