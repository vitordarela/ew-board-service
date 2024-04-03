using Domain.Model;
using Domain.Model.DTO.User;

namespace Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> AddUserAsync(UserRequest userRequest);

        Task<User> GetUserByIdAsync(string userId);
    }
}
