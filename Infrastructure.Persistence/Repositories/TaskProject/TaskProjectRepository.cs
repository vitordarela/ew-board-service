using Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

        public async Task<IEnumerable<TaskProject>> GetAllAsync()
        {
            return await _tasksBoard.Where(t => true).ToListAsync();
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

        public async Task UpdateAsync(TaskProject task)
        {
            _tasksBoard.Update(task);
            dbContext.SaveChanges();
        }

        public async Task DeleteAsync(string id)
        {
            var taskSearch = await _tasksBoard.FirstOrDefaultAsync(p => p.Id == id);
            _tasksBoard.Remove(taskSearch);
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<TaskProject>> GetAllByUserIdAsync(string userId)
        {
            return await _tasksBoard.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}
