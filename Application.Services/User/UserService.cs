using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.User;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await this.userRepository.GetAllAsync();
        }

        public async Task<User> AddUserAsync(UserRequest userRequest)
        {
            var user = this.mapper.Map<User>(userRequest);
            return await this.userRepository.AddAsync(user);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await this.userRepository.GetUserById(userId);
        }
    }
}
