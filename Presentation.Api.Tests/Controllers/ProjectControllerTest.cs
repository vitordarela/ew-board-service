using Application.Services;
using Domain.Model;
using Domain.Model.DTO.Project;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;


namespace Presentation.Api.Tests.Controllers
{
    public class ProjectControllerTest
    {
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly Mock<ITaskProjectService> _mockTaskProjectService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly ProjectController _controller;

        public ProjectControllerTest()
        {
            _mockProjectService = new Mock<IProjectService>();
            _mockTaskProjectService = new Mock<ITaskProjectService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new ProjectController(_mockProjectService.Object, _mockTaskProjectService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetProjectsAsync_ReturnsOkResult()
        {
            // Arrange
            string userId = "userId";
            var projects = new List<ProjectResponse>();
            _mockProjectService.Setup(x => x.GetAllProjectsAsync(userId)).ReturnsAsync(projects);

            // Act
            var result = await _controller.GetProjectsAsync(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task CreateProjectAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var projectRequest = new ProjectRequest { UserId = "userId" };
            var createdProject = new Project { Id = "1" };
            _mockUserService.Setup(x => x.GetUserByIdAsync(projectRequest.UserId)).ReturnsAsync(new User());
            _mockProjectService.Setup(x => x.AddProjectAsync(projectRequest)).ReturnsAsync(createdProject);

            // Act
            var result = await _controller.CreateProjectAsync(projectRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
        }


        [Fact]
        public async Task DeleteProjectAsync_WithPendingTasks_ReturnsBadRequest()
        {
            // Arrange
            string userId = "userId";
            string projectId = "projectId";
            _mockTaskProjectService.Setup(x => x.GetTaskNotCompletedAsync(projectId)).ReturnsAsync(new List<TaskProject> { new TaskProject() });

            // Act
            var result = await _controller.DeleteProjectAsync(userId, projectId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProjectAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            string projectId = "projectId";
            var projectRequest = new ProjectRequest();
            var updatedProject = new ProjectResponse { Id = projectId };
            _mockProjectService.Setup(x => x.UpdateProjectAsync(projectId, projectRequest)).ReturnsAsync(updatedProject);

            // Act
            var result = await _controller.UpdateTaskAsync(projectId, projectRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);
        }
    }
}
