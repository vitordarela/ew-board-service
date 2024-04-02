using AutoMapper;
using Domain.Model;
using Domain.Model.DTO.Project;
using Domain.Model.DTO.TaskBoard;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class TaskProjectService : ITaskProjectService
    {
        private readonly ITaskProjectRepository _taskBoardRepository;
        private readonly IMapper _mapper;

        public TaskProjectService(ITaskProjectRepository taskBoardRepository, IMapper mapper)
        {
            _taskBoardRepository = taskBoardRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskProject>> GetAllTaskBoardsByProjectIdAsync(string projectId)
        {
            return await _taskBoardRepository.GetByProjectIdAsync(projectId);
        }

        public Task<TaskProject> GetTaskBoardByIdAsync(string id)
        {
            return _taskBoardRepository.GetByIdAsync(id);
        }

        public async Task<TaskProject> AddTaskBoardAsync(string projectId, TaskProjectRequest taskProjectRequest)
        {
            var taskProject = _mapper.Map<TaskProject>(taskProjectRequest);
            taskProject.ProjectId = projectId;

            return await _taskBoardRepository.AddAsync(taskProject);
        }

        public Task UpdateTaskBoardAsync(TaskProject taskProject)
        {
            return _taskBoardRepository.UpdateAsync(taskProject);
        }

        public Task DeleteTaskBoardAsync(string id)
        {
            return _taskBoardRepository.DeleteAsync(id);
        }
    }
}
