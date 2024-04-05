using Domain.Model;
using Domain.Model.Enum;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskProjectRepository : ITaskProjectRepository
    {
        private readonly DbSet<TaskProject> _tasksProject;
        private readonly DbSet<TaskProjectHistory> _taskProjectHistory;
        private readonly MongoDbContext dbContext;

        public TaskProjectRepository(MongoDbContext dbContext)
        {
            _tasksProject = dbContext.TaskProject;
            _taskProjectHistory = dbContext.TaskProjectHistory;
            this.dbContext = dbContext;
        }

        public async Task<TaskProject> GetByIdAsync(string id)
        {
            return await _tasksProject.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskProject>> GetByProjectIdAsync(string projectId)
        {
            var taskProjects = await _tasksProject
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();

            foreach (var taskProject in taskProjects)
            {
                taskProject.History = await _taskProjectHistory
                    .Where(th => th.TaskId == taskProject.Id)
                    .ToListAsync();
            }

            return taskProjects;
        }

        public async Task<TaskProject> AddAsync(TaskProject task)
        {
            var taskSaved = await _tasksProject.AddAsync(task);
            dbContext.SaveChanges();

            return taskSaved.Entity;
        }

        public async Task<TaskProject> UpdateAsync(TaskProject task)
        {
            var existingTask = await _tasksProject.FirstOrDefaultAsync(t => t.Id == task.Id & t.ProjectId == task.ProjectId & t.UserId == task.UserId);

            if (existingTask != null)
            {
                existingTask.Status = task.Status;
                existingTask.DueDate = task.DueDate;
                existingTask.Description = task.Description;
                existingTask.Title = task.Title;
                existingTask.UserId = task.UserId;
                dbContext.SaveChanges();
            }

            return existingTask;
        }

        public async Task DeleteAsync(string projectId ,string taskId)
        {
            var taskSearch = await _tasksProject.FirstOrDefaultAsync(p => p.Id == taskId & p.ProjectId == projectId);
            _tasksProject.Remove(taskSearch);
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<TaskProject>> GetNotCompletedAsync(string projectId)
        {
            return await _tasksProject.Where(p => p.ProjectId == projectId & p.Status != TaskProjectStatus.Completed).ToListAsync();
        }

        public async Task<int> GetGeneralStatisticAsync(DateTime startDate, DateTime endDate, TaskProjectStatus? taskProjectStatus, TaskProjectPriority? taskProjectPriority)
        {
            var tasksQuery = _tasksProject.Where(t => t.DueDate >= startDate && t.DueDate <= endDate);

            if (taskProjectStatus != null)
                tasksQuery = tasksQuery.Where(t => t.Status == taskProjectStatus.Value);

            if (taskProjectPriority != null)
                tasksQuery = tasksQuery.Where(t => t.Priority == taskProjectPriority.Value);

            var tasksCount = await tasksQuery.CountAsync();

            return tasksCount;
        }

        public async Task<IEnumerable<TaskProject>> GetTasksPerDateAndStatusAsync(DateTime startDate, DateTime endDate, TaskProjectStatus taskProjectStatus)
        {
            var result = await _tasksProject
                .Where(t => t.Status == taskProjectStatus && t.DueDate >= startDate && t.DueDate <= endDate).ToListAsync();

            return result;
        }
    }
}
