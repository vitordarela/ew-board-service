﻿using Domain.Model;
using Domain.Model.DTO.Project;

namespace Application.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(string userId);

        Task<Project> GetProjectByIdAsync(string projectId);

        Task<Project> AddProjectAsync(ProjectRequest projectDTO);

        Task UpdateProjectAsync(ProjectRequest projectDTO);

        Task DeleteProjectAsync(string userId, string projectId);
     }
}
