using Domain.Model;
using Domain.Model.DTO.Report;
using Domain.Model.DTO.TaskBoard;
using Domain.Model.Enum;

namespace Application.Services
{
    public interface ITaskProjectService
    {
        Task<IEnumerable<TaskProject>> GetAllTaskBoardsByProjectIdAsync(string projectId);

        Task<TaskProject> GetTaskBoardByIdAsync(string projectId);

        Task<TaskProject> AddTaskBoardAsync(string projectId, TaskProjectRequest taskProjectRequest);

        Task<TaskProject> UpdateTaskBoardAsync(string projectId, string taskId, TaskProjectUpdateRequest taskProjectUpdateRequest);

        Task DeleteTaskBoardAsync(string projectId, string taskId);

        Task<bool> ReachedTaskLimit(string projectId);

        Task<IEnumerable<TaskProject>> GetTaskNotCompletedAsync(string projectId);

        Task<int> GetGeneralStatisticAsync(DateTime startDate, DateTime endDate, TaskProjectStatus? taskProjectStatus, TaskProjectPriority? taskProjectPriority);

        Task<AverageReportResult> GetAverageTasksByStatusAsync(DateTime startDate, DateTime endDate, TaskProjectStatus taskProjectStatus);
    }
}
