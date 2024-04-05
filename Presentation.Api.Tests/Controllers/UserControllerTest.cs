using Application.Services;
using Domain.Model;
using Domain.Model.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;

namespace Presentation.Api.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsOkResult()
        {
            // Arrange
            var users = new List<User>();
            _mockUserService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsersAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task CreateUserAsync_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var userRequest = new UserRequest();
            var createdUser = new User { Id = "1" };
            _mockUserService.Setup(x => x.AddUserAsync(userRequest)).ReturnsAsync(createdUser);

            // Act
            var result = await _controller.CreateUserAsync(userRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
