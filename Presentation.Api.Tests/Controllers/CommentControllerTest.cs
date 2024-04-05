using Application.Services;
using Domain.Model;
using Domain.Model.DTO.Comment;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;


namespace Presentation.Api.Tests.Controllers
{
    public class CommentControllerTest
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly Mock<ITaskProjectService> _mockTaskProjectService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CommentController _controller;

        public CommentControllerTest()
        {
            _mockCommentService = new Mock<ICommentService>();
            _mockTaskProjectService = new Mock<ITaskProjectService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new CommentController(_mockCommentService.Object, _mockTaskProjectService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetCommentsAsync_ReturnsOkResult()
        {
            // Arrange
            string taskId = "taskId";
            var comments = new List<CommentResponse>();
            _mockCommentService.Setup(x => x.GetAllCommentsAsync(taskId)).ReturnsAsync(comments);

            // Act
            var result = await _controller.GetCommentsAsync(taskId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task CreateCommentAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            string taskId = "taskId";
            var commentRequest = new CommentRequest { UserId = "userId", Description = "Test comment" };

            _mockUserService.Setup(x => x.GetUserByIdAsync(commentRequest.UserId)).ReturnsAsync(new User());
            _mockTaskProjectService.Setup(x => x.GetTaskBoardByIdAsync(taskId)).ReturnsAsync(new TaskProject());

            var createdComment = new Comment { Id = "1" };

            _mockCommentService.Setup(x => x.AddCommentAsync(taskId, commentRequest)).ReturnsAsync(createdComment);

            // Act
            var result = await _controller.CreateCommentAsync(commentRequest, taskId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task DeleteCommentAsync_WithValidData_ReturnsNoContentResult()
        {
            // Arrange
            string userId = "userId";
            string commentId = "commentId";
            string taskId = "taskId";

            // Act
            var result = await _controller.DeleteCommentAsync(userId, commentId, taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCommentAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            string commentId = "commentId";
            string taskId = "taskId";
            var commentRequest = new CommentRequest { UserId = "userId", Description = "Updated comment" };
            var updatedComment = new CommentResponse { Id = commentId, Description = "Updated comment" };
            _mockCommentService.Setup(x => x.UpdateCommentAsync(commentId, taskId, commentRequest)).ReturnsAsync(updatedComment);

            // Act
            var result = await _controller.UpdateCommentAsync(commentId, taskId, commentRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
        }
    }
}
