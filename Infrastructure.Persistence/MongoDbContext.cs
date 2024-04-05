using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Domain.Model.Common;

namespace Infrastructure.Persistence
{
    public class MongoDbContext : DbContext
    {
        public DbSet<Project> Projects { get; init; }
        public DbSet<TaskProject> TaskProject { get; init; }
        public DbSet<Comment> Comment { get; init; }
        public DbSet<User> User { get; init; }
        public DbSet<TaskProjectHistory> TaskProjectHistory { get; init; }

        public MongoDbContext(DbContextOptions options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project>().ToCollection("projects");
            modelBuilder.Entity<TaskProject>().ToCollection("tasks").HasMany(tp => tp.History).WithOne().HasForeignKey(tp => tp.TaskId);
            modelBuilder.Entity<Comment>().ToCollection("comments");
            modelBuilder.Entity<User>().ToCollection("users");
            modelBuilder.Entity<TaskProjectHistory>().ToCollection("task_history");
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Modified || x.State == EntityState.Added) && x.Metadata.GetCollectionName() != "task_history");

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as BaseModelEntity;
                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                    }

                    entity.UpdatedAt = DateTime.UtcNow;
                }
                this.LogHistoryInformation(entry);
                break;
            }

            return base.SaveChanges();
        }

        private void LogHistoryInformation(EntityEntry entry)
        {
            if (entry != null)
            {
                if ("Domain.Model.TaskProject".Equals(entry.Metadata.Name))
                {
                    var taskProject = entry.Entity as TaskProject;

                    var taskHistory = new TaskProjectHistory
                    {
                        CollectionName = entry.Metadata.Name,
                        Action = entry.State.ToString(),
                        NewValues = JsonConvert.SerializeObject(entry.Entity),
                        DateTime = DateTime.UtcNow,
                        UserId = taskProject.UserId,
                        TaskId = taskProject.Id,
                    };

                    var mongoDbContext = (MongoDbContext)this;
                    mongoDbContext.TaskProjectHistory.AddAsync(taskHistory);
                }

                if ("Domain.Model.Comment".Equals(entry.Metadata.Name))
                {
                    var comment = entry.Entity as Comment;

                    var taskHistory = new TaskProjectHistory
                    {
                        CollectionName = entry.Metadata.Name,
                        Action = entry.State.ToString(),
                        NewValues = JsonConvert.SerializeObject(entry.Entity),
                        DateTime = DateTime.UtcNow,
                        UserId = comment.UserId,
                        TaskId = comment.TaskId,
                    };

                    var mongoDbContext = (MongoDbContext)this;
                    mongoDbContext.TaskProjectHistory.AddAsync(taskHistory);
                }

            }   
        }
    }
}
