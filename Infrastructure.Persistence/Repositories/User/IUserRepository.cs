using Domain.Model;

namespace Infrastructure.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> AddAsync(User user);

        Task<User> GetUserById(string userId);
    }
}
