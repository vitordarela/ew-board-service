using Application.Services;
using Domain.Model.DTO.TaskBoard;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/Project/{projectId}/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskProjectService taskProjectService;

        public TaskController(ITaskProjectService taskProjectService)
        {
            this.taskProjectService = taskProjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksForProjectAsync(string projectId)
        {
            var tasks = await this.taskProjectService.GetAllTaskBoardsByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        // Criação de Tarefas - adicionar uma nova tarefa a um projeto
        [HttpPost]
        public async Task<IActionResult> CreateTaskForProjectAsync(string projectId, [FromBody] TaskProjectRequest taskProjectRequest)
        {
            bool reachedLimit = await this.taskProjectService.ReachedTaskLimit(projectId).ConfigureAwait(false);
            
            if (reachedLimit)
            {
                return BadRequest(new { ErrorMessage = "You have reached the maximum limit of 20 tasks for this project." } );
            }


            var createdTask = await this.taskProjectService.AddTaskBoardAsync(projectId, taskProjectRequest).ConfigureAwait(false);
            return Ok(new { id = createdTask.Id });
        }

        // Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(string projectId, string taskId, [FromBody] TaskProjectUpdateRequest taskProjectUpdateRequest)
        {
            var taskUpdated = await this.taskProjectService.UpdateTaskBoardAsync(projectId, taskId, taskProjectUpdateRequest);

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
    }
}

