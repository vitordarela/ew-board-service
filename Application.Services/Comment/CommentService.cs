using Application.Services.Interfaces;
using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Comment;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CommentResponse>> GetAllCommentsAsync(string taskId)
        {
            var comments = await this.commentRepository.GetAllAsync(taskId);

            return this.mapper.Map<IEnumerable<CommentResponse>>(comments);
        }

        public async Task<Comment> AddCommentAsync(string taskId, CommentRequest commentRequest)
        {
            var comment = mapper.Map<Comment>(commentRequest);
            comment.TaskId = taskId;

            return await this.commentRepository.AddAsync(comment);
        }

        public async Task<CommentResponse> UpdateCommentAsync(string commentId, string taskId, CommentRequest commentRequest)
        {
            var comment = mapper.Map<Comment>(commentRequest);
            comment.TaskId = taskId;
            comment.Id = commentId;

            var commentUpdated = await this.commentRepository.UpdateAsync(comment);

            return this.mapper.Map<CommentResponse>(commentUpdated);
        }

        public async Task DeleteCommentAsync(string userId, string commentId, string taskId)
        {
            await this.commentRepository.DeleteAsync(userId, commentId, taskId);
        }
    }
}
