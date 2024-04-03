using Application.Services;
using Domain.Model.DTO.Project;
using Domain.Model.DTO.TaskBoard;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ITaskProjectService taskProjectService;
        private readonly IUserService userService;

        public ProjectController(IProjectService projectService, ITaskProjectService taskProjectService, IUserService userService)
        {
            this.projectService = projectService;
            this.taskProjectService = taskProjectService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync([FromQuery] string userId)
        {
            var projects = await this.projectService.GetAllProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectRequest projectRequest)
        {
            var userExist = await UserExist(projectRequest.UserId);

            if (!userExist)
            {
                return BadRequest(new { ErrorMessage = "User does not exist" });
            }

            var createdProject = await this.projectService.AddProjectAsync(projectRequest);

            return Ok(new { id = createdProject.Id });
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProjectAsync([FromQuery] string userId, string projectId)
        {
            if (await this.HasPendingTasksAsync(projectId))
            {
                return BadRequest(new { errorMessage = "The project cannot be removed because there are pending tasks associated with it" });
            }
            else
            {
                await this.projectService.DeleteProjectAsync(userId, projectId);
                return NoContent();
            }
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateTaskAsync(string projectId, [FromBody] ProjectRequest projectRequest)
        {
            var projectUpdated = await this.projectService.UpdateProjectAsync(projectId, projectRequest);

            if (projectUpdated == null)
            {
                return NotFound(new { ErrorMessage = "Project not Found" });
            }

            return Ok(projectUpdated);
        }

        private async Task<bool> HasPendingTasksAsync(string projectId)
        {
            var tasks = await this.taskProjectService.GetTaskNotCompletedAsync(projectId).ConfigureAwait(false);
            return tasks.Count() > 0;
        }

        private async Task<bool> UserExist(string userId)
        {
            var user = await this.userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            return user != null;
        }
    }
}

