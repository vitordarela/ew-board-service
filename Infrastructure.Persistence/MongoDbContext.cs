using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using Domain.Model;

namespace Infrastructure.Persistence
{
    public class MongoDbContext : DbContext
    {
        public DbSet<Project> Projects { get; init; }
        public DbSet<TaskProject> TaskBoard { get; init; }

        public MongoDbContext(DbContextOptions options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project>().ToCollection("projects");
            modelBuilder.Entity<TaskProject>().ToCollection("tasks");
        }
    }
}
