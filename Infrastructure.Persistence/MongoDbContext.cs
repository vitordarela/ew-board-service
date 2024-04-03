using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Domain.Model;

namespace Infrastructure.Persistence
{
    public class MongoDbContext : DbContext
    {
        public DbSet<Project> Projects { get; init; }
        public DbSet<TaskProject> TaskBoard { get; init; }
        public DbSet<Comment> Comment { get; init; }
        public DbSet<User> User { get; init; }

        public MongoDbContext(DbContextOptions options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project>().ToCollection("projects");
            modelBuilder.Entity<TaskProject>().ToCollection("tasks");
            modelBuilder.Entity<Comment>().ToCollection("comments");
            modelBuilder.Entity<User>().ToCollection("users");
        }
    }
}
