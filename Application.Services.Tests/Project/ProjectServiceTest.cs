using Application.Services.Interfaces;
using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Project;
using Domain.Model.Mapping;
using Moq;

namespace Application.Services.Tests
{
    public class ProjectServiceTest
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly IMapper _mapper;
        private readonly ProjectService _projectService;

        public ProjectServiceTest()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _projectService = new ProjectService(_mockProjectRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsListOfProjects()
        {
            // Arrange
            string userId = "userId";
            var projects = new List<Project>
            {
                new Project { Id = "1", Name = "Project 1" },
                new Project { Id = "2", Name = "Project 2" }
            };
            _mockProjectRepository.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(projects);

            // Act
            var result = await _projectService.GetAllProjectsAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ProjectResponse>>(result);
            Assert.Equal(projects.Count, ((List<ProjectResponse>)result).Count);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ReturnsProject()
        {
            // Arrange
            string projectId = "projectId";
            var project = new Project { Id = projectId, Name = "Test project" };
            _mockProjectRepository.Setup(x => x.GetByIdAsync(projectId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Project>(result);
            Assert.Equal(projectId, result.Id);
        }

        [Fact]
        public async Task AddProjectAsync_ReturnsAddedProject()
        {
            // Arrange
            var projectRequest = new ProjectRequest { Name = "New project" };
            var project = new Project { Name = projectRequest.Name };
            _mockProjectRepository.Setup(x => x.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);

            // Act
            var result = await _projectService.AddProjectAsync(projectRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Project>(result);
            Assert.Equal(project.Name, result.Name);
        }

        [Fact]
        public async Task UpdateProjectAsync_ReturnsUpdatedProject()
        {
            // Arrange
            string projectId = "projectId";
            var projectRequest = new ProjectRequest { Name = "Updated project" };
            var project = new Project { Id = projectId, Name = projectRequest.Name };
            _mockProjectRepository.Setup(x => x.UpdateAsync(It.IsAny<Project>())).ReturnsAsync(project);

            // Act
            var result = await _projectService.UpdateProjectAsync(projectId, projectRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectResponse>(result);
            Assert.Equal(projectId, result.Id);
            Assert.Equal(projectRequest.Name, result.Name);
        }

        [Fact]
        public async Task DeleteProjectAsync_DeletesProject()
        {
            // Arrange
            string userId = "userId";
            string projectId = "projectId";

            // Act
            await _projectService.DeleteProjectAsync(userId, projectId);

            // Assert
            _mockProjectRepository.Verify(x => x.DeleteAsync(userId, projectId), Times.Once);
        }
    }
}
