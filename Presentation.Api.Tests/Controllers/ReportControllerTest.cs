using Application.Services;
using Domain.Model;
using Domain.Model.DTO.Report;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;

namespace Presentation.Api.Tests.Controllers
{
    public class ReportControllerTest
    {
        private readonly Mock<ITaskProjectService> _mockTaskProjectService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly ReportController _controller;

        public ReportControllerTest()
        {
            _mockTaskProjectService = new Mock<ITaskProjectService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new ReportController(_mockTaskProjectService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetGeneralStatisticAsync_WithManagerRole_ReturnsOkResult()
        {
            // Arrange
            string userId = "managerUserId";
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var taskProjectStatus = TaskProjectStatus.InProgress;
            var taskProjectPriority = TaskProjectPriority.High;
            var expectedCount = 10;

            _mockUserService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(new User { Role = Role.Manager });
            _mockTaskProjectService.Setup(x => x.GetGeneralStatisticAsync(startDate, endDate, taskProjectStatus, taskProjectPriority))
                                    .ReturnsAsync(expectedCount);

            // Act
            var result = await _controller.GetGeneralStatisticAsync(userId, startDate, endDate, taskProjectStatus, taskProjectPriority);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reportResult = Assert.IsType<GeneralReportResult>(okResult.Value);
            Assert.Equal(expectedCount, reportResult.Count);
        }

        // Add more tests for GetGeneralStatisticAsync to cover different scenarios

        [Fact]
        public async Task GetAverageTasksByStatusAsync_WithManagerRole_ReturnsOkResult()
        {
            // Arrange
            string userId = "managerUserId";
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var status = TaskProjectStatus.InProgress;
            var averageReportResult = new AverageReportResult
            {
                AverageTasksPerUser = 5,
            };

            _mockUserService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(new User { Role = Role.Manager });
            _mockTaskProjectService.Setup(x => x.GetAverageTasksByStatusAsync(startDate, endDate, status))
                                    .ReturnsAsync(averageReportResult);

            // Act
            var result = await _controller.GetAverageTasksByStatusAsync(userId, startDate, endDate, status);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reportResult = Assert.IsType<AverageReportResult>(okResult.Value);
            Assert.Equal(averageReportResult.AverageTasksPerUser, reportResult.AverageTasksPerUser);
        }
    }
}
