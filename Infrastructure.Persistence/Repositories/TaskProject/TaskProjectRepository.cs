using Domain.Model;
using Domain.Model.Enum;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskProjectRepository : ITaskProjectRepository
    {
        private readonly DbSet<TaskProject> _tasksBoard;
        private readonly MongoDbContext dbContext;

        public TaskProjectRepository(MongoDbContext dbContext)
        {
            _tasksBoard = dbContext.TaskBoard;
            this.dbContext = dbContext;
        }

        public async Task<TaskProject> GetByIdAsync(string id)
        {
            return await _tasksBoard.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskProject>> GetByProjectIdAsync(string projectId)
        {
            return await _tasksBoard.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskProject> AddAsync(TaskProject task)
        {
            var taskSaved = await _tasksBoard.AddAsync(task);
            dbContext.SaveChanges();

            return taskSaved.Entity;
        }

        public async Task<TaskProject> UpdateAsync(TaskProject task)
        {
            var existingTask = await _tasksBoard.FirstOrDefaultAsync(t => t.Id == task.Id & t.ProjectId == task.ProjectId);

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
            var taskSearch = await _tasksBoard.FirstOrDefaultAsync(p => p.Id == taskId & p.ProjectId == projectId);
            _tasksBoard.Remove(taskSearch);
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<TaskProject>> GetNotCompletedAsync(string projectId)
        {
            return await _tasksBoard.Where(p => p.ProjectId == projectId & p.Status != TaskProjectStatus.Completed).ToListAsync();
        }
    }
}
