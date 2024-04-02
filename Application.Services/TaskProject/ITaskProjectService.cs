using Domain.Model;
using Domain.Model.DTO.TaskBoard;

namespace Application.Services
{
    public interface ITaskProjectService
    {
        Task<IEnumerable<TaskProject>> GetAllTaskBoardsByProjectIdAsync(string projectId);

        Task<TaskProject> GetTaskBoardByIdAsync(string id);

        Task<TaskProject> AddTaskBoardAsync(string projectId, TaskProjectRequest taskProjectRequest);

        Task UpdateTaskBoardAsync(TaskProject taskBoard);

        Task DeleteTaskBoardAsync(string id);
    }
}
