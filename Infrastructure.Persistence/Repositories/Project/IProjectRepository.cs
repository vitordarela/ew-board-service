using Domain.Model;

namespace Application.Services.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(string userId);

        Task<Project> GetByIdAsync(string id);

        Task<Project> AddAsync(Project project);

        Task UpdateAsync(Project project);

        Task DeleteAsync(string id);
    }
}
