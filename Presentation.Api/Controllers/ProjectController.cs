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

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
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
        public IActionResult DeleteProject([FromQuery] string userId, string projectId)
        {
            //if (this.projectService.HasPendingTasks(projectId))
            //{
            //    return BadRequest("O projeto não pode ser removido porque há tarefas pendentes associadas a ele.");
            //}
            //else
            //{
                this.projectService.DeleteProjectAsync(projectId);
                return NoContent();
            //}
        }
    }
}

