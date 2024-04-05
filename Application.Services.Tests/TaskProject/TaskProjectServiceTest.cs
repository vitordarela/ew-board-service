using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Report;
using Domain.Model.DTO.TaskBoard;
using Domain.Model.Enum;
using Domain.Model.Mapping;
using Infrastructure.Persistence.Repositories;
using Moq;

namespace Application.Services.Tests
{
    public class TaskProjectServiceTest
    {
        private readonly Mock<ITaskProjectRepository> _mockTaskProjectRepository;
        private readonly IMapper _mapper;
        private readonly TaskProjectService _taskProjectService;

        public TaskProjectServiceTest()
        {
            _mockTaskProjectRepository = new Mock<ITaskProjectRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _taskProjectService = new TaskProjectService(_mockTaskProjectRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllTaskBoardsByProjectIdAsync_ReturnsListOfTaskBoards()
        {
            // Arrange
            string projectId = "projectId";
            var taskBoards = new List<TaskProject>
            {
                new TaskProject { Id = "1", Title = "Task 1" },
                new TaskProject { Id = "2", Title = "Task 2" }
            };
            _mockTaskProjectRepository.Setup(x => x.GetByProjectIdAsync(projectId)).ReturnsAsync(taskBoards);

            // Act
            var result = await _taskProjectService.GetAllTaskBoardsByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TaskProject>>(result);
            Assert.Equal(taskBoards.Count, ((List<TaskProject>)result).Count);
        }

        [Fact]
        public async Task AddTaskBoardAsync_ReturnsAddedTaskBoard()
        {
            // Arrange
            string projectId = "projectId";
            var taskProjectRequest = new TaskProjectRequest { Title = "New task", UserId = "userId" };
            var taskProject = new TaskProject { Title = taskProjectRequest.Title, UserId = taskProjectRequest.UserId };
            _mockTaskProjectRepository.Setup(x => x.AddAsync(It.IsAny<TaskProject>())).ReturnsAsync(taskProject);

            // Act
            var result = await _taskProjectService.AddTaskBoardAsync(projectId, taskProjectRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TaskProject>(result);
            Assert.Equal(taskProject.Title, result.Title);
            Assert.Equal(taskProject.UserId, result.UserId);
        }

        [Fact]
        public async Task UpdateTaskBoardAsync_ReturnsUpdatedTaskBoard()
        {
            // Arrange
            string projectId = "projectId";
            string taskId = "taskId";
            var taskProjectUpdateRequest = new TaskProjectUpdateRequest { Title = "Updated task" };
            var taskProject = new TaskProject { Id = taskId, Title = taskProjectUpdateRequest.Title, ProjectId = projectId };
            _mockTaskProjectRepository.Setup(x => x.UpdateAsync(It.IsAny<TaskProject>())).ReturnsAsync(taskProject);

            // Act
            var result = await _taskProjectService.UpdateTaskBoardAsync(projectId, taskId, taskProjectUpdateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TaskProject>(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal(taskProjectUpdateRequest.Title, result.Title);
            Assert.Equal(projectId, result.ProjectId);
        }

        [Fact]
        public async Task DeleteTaskBoardAsync_DeletesTaskBoard()
        {
            // Arrange
            string projectId = "projectId";
            string taskId = "taskId";

            // Act
            await _taskProjectService.DeleteTaskBoardAsync(projectId, taskId);

            // Assert
            _mockTaskProjectRepository.Verify(x => x.DeleteAsync(projectId, taskId), Times.Once);
        }

        [Fact]
        public async Task ReachedTaskLimit_ReturnsTrueWhenLimitReached()
        {
            // Arrange
            string projectId = "projectId";
            var taskBoards = Enumerable.Range(1, 20).Select(i => new TaskProject()).ToList();
            _mockTaskProjectRepository.Setup(x => x.GetByProjectIdAsync(projectId)).ReturnsAsync(taskBoards);

            // Act
            var result = await _taskProjectService.ReachedTaskLimit(projectId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetGeneralStatisticAsync_ReturnsStatistic()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var status = TaskProjectStatus.InProgress;
            var priority = TaskProjectPriority.High;
            int expectedResult = 10;
            _mockTaskProjectRepository.Setup(x => x.GetGeneralStatisticAsync(startDate, endDate, status, priority)).ReturnsAsync(expectedResult);

            // Act
            var result = await _taskProjectService.GetGeneralStatisticAsync(startDate, endDate, status, priority);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetAverageTasksByStatusAsync_ReturnsAverageReportResult()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var status = TaskProjectStatus.InProgress;
            var tasks = new List<TaskProject>
            {
                new TaskProject { UserId = "user1" },
                new TaskProject { UserId = "user1" },
                new TaskProject { UserId = "user2" }
            };
            _mockTaskProjectRepository.Setup(x => x.GetTasksPerDateAndStatusAsync(startDate, endDate, status)).ReturnsAsync(tasks);

            // Act
            var result = await _taskProjectService.GetAverageTasksByStatusAsync(startDate, endDate, status);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AverageReportResult>(result);
            Assert.Equal(1.5, result.AverageTasksPerUser);
        }

        [Fact]
        public async Task GetTaskHistoryByTaskIdAsync_ReturnsTaskHistory()
        {
            // Arrange
            var taskId = "taskId";
            var expectedTaskHistory = new List<TaskProjectHistory>();
            var mockTaskProjectRepository = new Mock<ITaskProjectRepository>();
            mockTaskProjectRepository.Setup(x => x.GetTaskHistoryByTaskIdAsync(taskId)).ReturnsAsync(expectedTaskHistory);


            // Act
            var result = await _taskProjectService.GetTaskHistoryByTaskIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTaskHistory, result);
        }
    }
}
