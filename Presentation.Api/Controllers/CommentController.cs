using Application.Services;
using Domain.Model.DTO.Comment;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/task/{taskId}/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly ITaskProjectService taskProjectService;
        private readonly IUserService userService;

        public CommentController(ICommentService commentService, ITaskProjectService taskProjectService, IUserService userService)
        {
            this.commentService = commentService;
            this.taskProjectService = taskProjectService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsAsync(string taskId)
        {
            var comments = await this.commentService.GetAllCommentsAsync(taskId).ConfigureAwait(false);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CommentRequest commentRequest, string taskId)
        {
            var checkIfUserExist = await this.UserExistAsync(commentRequest.UserId);
            var checkIfTaskExist = await this.CheckIfTaskExist(taskId).ConfigureAwait(false);

            if (!checkIfTaskExist || !checkIfUserExist)
            {
                return BadRequest(new { ErrorMessage = "The task or user does not exist" });
            }

            var createdComment = await this.commentService.AddCommentAsync(taskId, commentRequest).ConfigureAwait(false);
            return Ok(new { id = createdComment.Id });
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync([FromQuery] string userId, string commentId, string taskId)
        {
            await this.commentService.DeleteCommentAsync(userId, commentId, taskId);
            return NoContent();
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateCommentAsync(string commentId, string taskId, [FromBody] CommentRequest commentRequest)
        {
            var taskUpdated = await this.commentService.UpdateCommentAsync(commentId, taskId, commentRequest).ConfigureAwait(false);

            if (taskUpdated == null)
            {
                return NotFound(new { ErrorMessage = "Comment not Found" });
            }

            return Ok(taskUpdated);
        }

        private async Task<bool> CheckIfTaskExist(string taskId)
        {
            var task = await this.taskProjectService.GetTaskBoardByIdAsync(taskId).ConfigureAwait(false);
            return task != null;
        }

        private async Task<bool> UserExistAsync(string userId)
        {
            var user = await this.userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            return user != null;
        }
    }
}
