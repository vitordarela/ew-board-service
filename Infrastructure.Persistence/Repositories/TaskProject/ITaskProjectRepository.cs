using Domain.Model;

namespace Infrastructure.Persistence.Repositories
{
    public interface ITaskProjectRepository
    {
        Task<IEnumerable<TaskProject>> GetAllAsync();

        Task<TaskProject> GetByIdAsync(string id);

        Task<IEnumerable<TaskProject>> GetByProjectIdAsync(string projectId);

        Task<IEnumerable<TaskProject>> GetAllByUserIdAsync(string userId);

        Task<TaskProject> AddAsync(TaskProject task);

        Task UpdateAsync(TaskProject task);

        Task DeleteAsync(string id);
    }
}
