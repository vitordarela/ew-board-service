using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Report;
using Domain.Model.DTO.TaskBoard;
using Domain.Model.Enum;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class TaskProjectService : ITaskProjectService
    {
        private readonly ITaskProjectRepository taskBoardRepository;
        private readonly IMapper mapper;

        public TaskProjectService(ITaskProjectRepository taskBoardRepository, IMapper mapper)
        {
            this.taskBoardRepository = taskBoardRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TaskProject>> GetAllTaskBoardsByProjectIdAsync(string projectId)
        {
            return await taskBoardRepository.GetByProjectIdAsync(projectId);
        }

        public Task<TaskProject> GetTaskBoardByIdAsync(string projectId)
        {
            return taskBoardRepository.GetByIdAsync(projectId);
        }

        public async Task<TaskProject> AddTaskBoardAsync(string projectId, TaskProjectRequest taskProjectRequest)
        {
            var taskProject = mapper.Map<TaskProject>(taskProjectRequest);
            taskProject.ProjectId = projectId;

            return await taskBoardRepository.AddAsync(taskProject);
        }

        public async Task<TaskProject> UpdateTaskBoardAsync(string projectId, string taskId, TaskProjectUpdateRequest taskProjectUpdateRequest)
        {
            var taskProject = mapper.Map<TaskProject>(taskProjectUpdateRequest);
            taskProject.ProjectId = projectId;
            taskProject.Id = taskId;

            return await taskBoardRepository.UpdateAsync(taskProject);
        }

        public Task DeleteTaskBoardAsync(string projectId, string taskId)
        {
            return taskBoardRepository.DeleteAsync(projectId, taskId);
        }

        public async Task<bool> ReachedTaskLimit(string projectId)
        {
            var tasks = await taskBoardRepository.GetByProjectIdAsync(projectId);
            int taskCount = tasks.Count();

            return taskCount >= 20;
        }

        public async Task<IEnumerable<TaskProject>> GetTaskNotCompletedAsync(string projectId)
        {
            return await this.taskBoardRepository.GetNotCompletedAsync(projectId);
        }

        public async Task<int> GetGeneralStatisticAsync(DateTime startDate, DateTime endDate, TaskProjectStatus? taskProjectStatus, TaskProjectPriority? taskProjectPriority)
        {
            return await this.taskBoardRepository.GetGeneralStatisticAsync(startDate, endDate, taskProjectStatus, taskProjectPriority);
        }

        public async Task<AverageReportResult> GetAverageTasksByStatusAsync(DateTime startDate, DateTime endDate, TaskProjectStatus taskProjectStatus)
        {
            var tasks = await this.taskBoardRepository.GetTasksPerDateAndStatusAsync(startDate, endDate, taskProjectStatus);

            var tasksPerUser = tasks
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalTasks = g.Count()
                })
                .ToList();

            double averageTasks = tasksPerUser.Any() ? tasksPerUser.Average(t => t.TotalTasks) : 0;

            return new AverageReportResult { AverageTasksPerUser = Math.Round(averageTasks, 2) };
        }
    }
}
