﻿using Domain.Model;
using Domain.Model.Enum;

namespace Infrastructure.Persistence.Repositories
{
    public interface ITaskProjectRepository
    {

        Task<TaskProject> GetByIdAsync(string id);

        Task<IEnumerable<TaskProject>> GetByProjectIdAsync(string projectId);

        Task<IEnumerable<TaskProjectHistory>> GetTaskHistoryByTaskIdAsync(string taskId);

        Task<IEnumerable<TaskProject>> GetNotCompletedAsync(string projectId);

        Task<TaskProject> AddAsync(TaskProject task);

        Task<TaskProject> UpdateAsync(TaskProject task);

        Task DeleteAsync(string projectId, string taskId);

        Task<int> GetGeneralStatisticAsync(DateTime startDate, DateTime endDate, TaskProjectStatus? taskProjectStatus, TaskProjectPriority? taskProjectPriority);

        Task<IEnumerable<TaskProject>> GetTasksPerDateAndStatusAsync(DateTime startDate, DateTime endDate, TaskProjectStatus taskProjectStatus);
    }
}
