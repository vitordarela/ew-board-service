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
            var createdTask = await this.taskProjectService.AddTaskBoardAsync(projectId, taskProjectRequest);
            return Ok(new { id = createdTask.Id });
        }

        // Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
        [HttpPut("{taskId}")]
        public IActionResult UpdateTask(string projectId, string taskId, [FromBody] TaskProjectRequest taskProjectRequest)
        {
            // Implementação para atualizar uma tarefa
            return Ok(taskProjectRequest);
        }

        // Remoção de Tarefas - remover uma tarefa de um projeto
        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(string projectId, string taskId)
        {
            // Implementação para remover uma tarefa
            return Ok();
        }
    }
}

