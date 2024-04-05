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
        private readonly ITaskProjectRepository taskProject;
        private readonly IMapper mapper;

        public TaskProjectService(ITaskProjectRepository taskProjectRepository, IMapper mapper)
        {
            this.taskProject = taskProjectRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TaskProject>> GetAllTaskBoardsByProjectIdAsync(string projectId)
        {
            return await this.taskProject.GetByProjectIdAsync(projectId);
        }

        public async Task<IEnumerable<TaskProjectHistory>> GetTaskHistoryByTaskIdAsync(string taskId)
        {
            return await this.taskProject.GetTaskHistoryByTaskIdAsync(taskId);
        }

        public Task<TaskProject> GetTaskBoardByIdAsync(string projectId)
        {
            return this.taskProject.GetByIdAsync(projectId);
        }

        public async Task<TaskProject> AddTaskBoardAsync(string projectId, TaskProjectRequest taskProjectRequest)
        {
            var taskProject = mapper.Map<TaskProject>(taskProjectRequest);
            taskProject.ProjectId = projectId;

            return await this.taskProject.AddAsync(taskProject);
        }

        public async Task<TaskProject> UpdateTaskBoardAsync(string projectId, string taskId, TaskProjectUpdateRequest taskProjectUpdateRequest)
        {
            var taskProject = mapper.Map<TaskProject>(taskProjectUpdateRequest);
            taskProject.ProjectId = projectId;
            taskProject.Id = taskId;

            return await this.taskProject.UpdateAsync(taskProject);
        }

        public Task DeleteTaskBoardAsync(string projectId, string taskId)
        {
            return this.taskProject.DeleteAsync(projectId, taskId);
        }

        public async Task<bool> ReachedTaskLimit(string projectId)
        {
            var tasks = await this.taskProject.GetByProjectIdAsync(projectId);
            int taskCount = tasks.Count();

            return taskCount >= 20;
        }

        public async Task<IEnumerable<TaskProject>> GetTaskNotCompletedAsync(string projectId)
        {
            return await this.taskProject.GetNotCompletedAsync(projectId);
        }

        public async Task<int> GetGeneralStatisticAsync(DateTime startDate, DateTime endDate, TaskProjectStatus? taskProjectStatus, TaskProjectPriority? taskProjectPriority)
        {
            return await this.taskProject.GetGeneralStatisticAsync(startDate, endDate, taskProjectStatus, taskProjectPriority);
        }

        public async Task<AverageReportResult> GetAverageTasksByStatusAsync(DateTime startDate, DateTime endDate, TaskProjectStatus taskProjectStatus)
        {
            var tasks = await this.taskProject.GetTasksPerDateAndStatusAsync(startDate, endDate, taskProjectStatus);

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
