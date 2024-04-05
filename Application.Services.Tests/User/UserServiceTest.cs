using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.User;
using Domain.Model.Mapping;
using Infrastructure.Persistence.Repositories;
using Moq;

namespace Application.Services.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Name = "User 1" },
                new User { Id = "2", Name = "User 2" }
            };
            _mockUserRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result);
            Assert.Equal(users.Count, ((List<User>)result).Count);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsAddedUser()
        {
            // Arrange
            var userRequest = new UserRequest { Name = "New user" };
            var user = new User { Name = userRequest.Name };
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.AddUserAsync(userRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(user.Name, result.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            string userId = "userId";
            var user = new User { Id = userId, Name = "Test user" };
            _mockUserRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(userId, result.Id);
        }
    }
}
