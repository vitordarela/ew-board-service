﻿using Application.Services.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DbSet<Project> _projects;
        private readonly MongoDbContext dbContext;

        public ProjectRepository(MongoDbContext dbContext)
        {
            _projects = dbContext.Projects;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(string userId)
        {
            return await _projects.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Project> GetByIdAsync(string id)
        {
            return await _projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddAsync(Project project)
        {
            var projectSaved = await _projects.AddAsync(project);
            dbContext.SaveChanges();

            return projectSaved.Entity;
        }

        public async Task UpdateAsync(Project project)
        {
            _projects.Update(project);
            dbContext.SaveChanges();
        }

        public async Task DeleteAsync(string id)
        {
            var projectSearch = await _projects.FirstOrDefaultAsync(p => p.Id == id);
            _projects.Remove(projectSearch);
            dbContext.SaveChanges();
        }
    }
}
