using Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _user;
        private readonly MongoDbContext dbContext;

        public UserRepository(MongoDbContext dbContext)
        {
            _user = dbContext.User;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _user.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _user.FirstOrDefaultAsync(t => t.Id == userId);
        }

        public async Task<User> AddAsync(User user)
        {
            var userSaved = await _user.AddAsync(user);
            dbContext.SaveChanges();

            return userSaved.Entity;
        }
    }
}
