using Application.Services;
using Domain.Model;
using Domain.Model.DTO.TaskBoard;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;


namespace Presentation.Api.Tests.Controllers
{
    public class TaskControllerTest
    {
        private readonly Mock<ITaskProjectService> _mockTaskProjectService;
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly TaskController _controller;

        public TaskControllerTest()
        {
            _mockTaskProjectService = new Mock<ITaskProjectService>();
            _mockProjectService = new Mock<IProjectService>();
            _controller = new TaskController(_mockTaskProjectService.Object, _mockProjectService.Object);
        }

        [Fact]
        public async Task GetTasksForProjectAsync_ReturnsOkResult()
        {
            // Arrange
            string projectId = "project1";
            var tasks = new List<TaskProject>();
            _mockTaskProjectService.Setup(x => x.GetAllTaskBoardsByProjectIdAsync(projectId)).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasksForProjectAsync(projectId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateTaskForProjectAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            string projectId = "project1";
            var taskRequest = new TaskProjectRequest();
            _mockProjectService.Setup(x => x.GetProjectByIdAsync(projectId)).ReturnsAsync(new Project());
            _mockTaskProjectService.Setup(x => x.AddTaskBoardAsync(projectId, taskRequest)).ReturnsAsync(new TaskProject());

            // Act
            var result = await _controller.CreateTaskForProjectAsync(projectId, taskRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task UpdateTaskAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            string projectId = "project1";
            string taskId = "task1";
            var taskUpdateRequest = new TaskProjectUpdateRequest();
            _mockTaskProjectService.Setup(x => x.UpdateTaskBoardAsync(projectId, taskId, taskUpdateRequest)).ReturnsAsync(new TaskProject());

            // Act
            var result = await _controller.UpdateTaskAsync(projectId, taskId, taskUpdateRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTask_WithValidData_ReturnsNoContentResult()
        {
            // Arrange
            string projectId = "project1";
            string taskId = "task1";

            // Act
            var result = await _controller.DeleteTask(projectId, taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetTaskHistoryAsync_ReturnsOkWithTaskHistory()
        {
            // Arrange
            var taskId = "taskId";
            var expectedTasks = new List<TaskProjectHistory>();
            var mockTaskProjectService = new Mock<ITaskProjectService>();
            mockTaskProjectService.Setup(x => x.GetTaskHistoryByTaskIdAsync(taskId)).ReturnsAsync(expectedTasks);


            // Act
            var result = await _controller.GetTaskHistoryAsync(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualTasks = Assert.IsAssignableFrom<IEnumerable<TaskProjectHistory>>(okResult.Value);
            Assert.Equal(expectedTasks, actualTasks);
        }
    }
}
