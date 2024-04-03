using Application.Services;
using Domain.Model.DTO.Project;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ITaskProjectService taskProjectService;

        public ProjectController(IProjectService projectService, ITaskProjectService taskProjectService)
        {
            this.projectService = projectService;
            this.taskProjectService = taskProjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync([FromQuery] string userId)
        {
            var projects = await this.projectService.GetAllProjectsAsync(userId);
            return Ok(projects);
        }

        // Criação de Projetos - criar um novo projeto
        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectRequest projectDTO)
        {
            var createdProject = await this.projectService.AddProjectAsync(projectDTO);
            return Ok(new { id = createdProject.Id });
        }

        // Remoção de Projetos - remover um projeto
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

        private async Task<bool> HasPendingTasksAsync(string projectId)
        {
            var tasks = await this.taskProjectService.GetTaskNotCompletedAsync(projectId).ConfigureAwait(false);
            return tasks.Count() > 0;
        }
    }
}

