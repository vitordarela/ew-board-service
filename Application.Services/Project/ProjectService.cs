using Application.Services.Interfaces;
using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Project;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(string userId)
        {
            var projects = await this.projectRepository.GetAllAsync(userId);
            return this.mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        public async Task<Project> GetProjectByIdAsync(string projectId)
        {
            return await this.projectRepository.GetByIdAsync(projectId);
        }

        public async Task<Project> AddProjectAsync(ProjectRequest projectRequest)
        {
            var project = this.mapper.Map<Project>(projectRequest);
            return await this.projectRepository.AddAsync(project);
        }

        public async Task<ProjectResponse> UpdateProjectAsync(string projectId, ProjectRequest projectRequest)
        {
            var project = this.mapper.Map<Project>(projectRequest);
            project.Id = projectId;

            var projectUpdated = await this.projectRepository.UpdateAsync(project);

            return this.mapper.Map<ProjectResponse>(projectUpdated);
        }

        public async Task DeleteProjectAsync(string userId,string projectId)
        {
            await this.projectRepository.DeleteAsync(userId, projectId);
        }
    }
}
