using Domain.Model;
using Domain.Model.DTO.Project;

namespace Application.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(string userId);

        Task<Project> GetProjectByIdAsync(string projectId);

        Task<Project> AddProjectAsync(ProjectRequest projectRequest);

        Task<ProjectResponse> UpdateProjectAsync(string projectId, ProjectRequest projectRequest);

        Task DeleteProjectAsync(string userId, string projectId);
    }
}
