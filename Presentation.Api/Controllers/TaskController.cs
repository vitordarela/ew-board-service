using Application.Services;
using Domain.Model.DTO.TaskBoard;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/project/{projectId}/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskProjectService taskProjectService;
        private readonly IProjectService projectService;

        public TaskController(ITaskProjectService taskProjectService, IProjectService projectService)
        {
            this.taskProjectService = taskProjectService;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksForProjectAsync(string projectId)
        {
            var tasks = await this.taskProjectService.GetAllTaskBoardsByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskForProjectAsync(string projectId, [FromBody] TaskProjectRequest taskProjectRequest)
        {
            bool projectExist = await this.ProjectExist(projectId).ConfigureAwait(false);

            if (!projectExist)
            {
                return BadRequest(new { ErrorMessage = "Project does not exist." });
            }

            bool reachedLimit = await this.taskProjectService.ReachedTaskLimit(projectId).ConfigureAwait(false);

            if (reachedLimit)
            {
                return BadRequest(new { ErrorMessage = "You have reached the maximum limit of 20 tasks for this project." });
            }

            var createdTask = await this.taskProjectService.AddTaskBoardAsync(projectId, taskProjectRequest).ConfigureAwait(false);
            return Ok(new { id = createdTask.Id });
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(string projectId, string taskId, [FromBody] TaskProjectUpdateRequest taskProjectUpdateRequest)
        {
            var taskUpdated = await this.taskProjectService.UpdateTaskBoardAsync(projectId, taskId, taskProjectUpdateRequest).ConfigureAwait(false);

            if (taskUpdated == null)
            {
                return NotFound(new { ErrorMessage = "Task not Found" });
            }

            return Ok(taskUpdated);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(string projectId, string taskId)
        {
            await this.taskProjectService.DeleteTaskBoardAsync(projectId, taskId).ConfigureAwait(false);
            return NoContent();
        }

        private async Task<bool> ProjectExist(string projectId)
        {
            var project = await this.projectService.GetProjectByIdAsync(projectId).ConfigureAwait(false);

            return project != null;
        }
    }
}

