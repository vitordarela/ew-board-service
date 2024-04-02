using Application.Services.Interfaces;
using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Project;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(string userId)
        {
            var projects = await _projectRepository.GetAllAsync(userId);
            return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        public async Task<Project> GetProjectByIdAsync(string id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<Project> AddProjectAsync(ProjectRequest projectRequest)
        {
            var project = _mapper.Map<Project>(projectRequest);
            return await _projectRepository.AddAsync(project);
        }

        public Task UpdateProjectAsync(ProjectRequest projectRequest)
        {
            var project = _mapper.Map<Project>(projectRequest);
            return _projectRepository.UpdateAsync(project);
        }

        public Task DeleteProjectAsync(string id)
        {
            return _projectRepository.DeleteAsync(id);
        }
    }
}
