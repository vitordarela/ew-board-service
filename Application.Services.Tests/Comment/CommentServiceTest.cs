using Application.Services.Interfaces;
using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Comment;
using Domain.Model.Mapping;
using Moq;

namespace Application.Services.Tests
{
    public class CommentServiceTest
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly IMapper _mapper;
        private readonly CommentService _commentService;

        public CommentServiceTest()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _commentService = new CommentService(_mockCommentRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllCommentsAsync_ReturnsListOfComments()
        {
            // Arrange
            string taskId = "taskId";
            var comments = new List<Comment>
            {
                new Comment { Id = "1", Description = "Comment 1" },
                new Comment { Id = "2", Description = "Comment 2" }
            };
            _mockCommentRepository.Setup(x => x.GetAllAsync(taskId)).ReturnsAsync(comments);

            // Act
            var result = await _commentService.GetAllCommentsAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(result);
            Assert.Equal(comments.Count, ((List<CommentResponse>)result).Count);
        }

        [Fact]
        public async Task AddCommentAsync_ReturnsAddedComment()
        {
            // Arrange
            string taskId = "taskId";
            var commentRequest = new CommentRequest { UserId = "userId", Description = "New comment" };
            var comment = new Comment { TaskId = taskId, Description = commentRequest.Description };
            _mockCommentRepository.Setup(x => x.AddAsync(It.IsAny<Comment>())).ReturnsAsync(comment);

            // Act
            var result = await _commentService.AddCommentAsync(taskId, commentRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Comment>(result);
            Assert.Equal(comment.Description, result.Description);
        }

        [Fact]
        public async Task UpdateCommentAsync_ReturnsUpdatedComment()
        {
            // Arrange
            string commentId = "commentId";
            string taskId = "taskId";
            var commentRequest = new CommentRequest { UserId = "userId", Description = "Updated comment" };
            var comment = new Comment { Id = commentId, TaskId = taskId, Description = commentRequest.Description };
            _mockCommentRepository.Setup(x => x.UpdateAsync(It.IsAny<Comment>())).ReturnsAsync(comment);

            // Act
            var result = await _commentService.UpdateCommentAsync(commentId, taskId, commentRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CommentResponse>(result);
            Assert.Equal(comment.Description, result.Description);
            Assert.Equal(comment.Id, result.Id);
        }


        [Fact]
        public async Task DeleteCommentAsync_DeletesComment()
        {
            // Arrange
            string userId = "userId";
            string commentId = "commentId";
            string taskId = "taskId";

            // Act
            await _commentService.DeleteCommentAsync(userId, commentId, taskId);

            // Assert
            _mockCommentRepository.Verify(x => x.DeleteAsync(userId, commentId, taskId), Times.Once);
        }
    }
}
