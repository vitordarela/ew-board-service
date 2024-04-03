using AutoMapper;
using Domain.Model.DTO.Project;
using Domain.Model.DTO.TaskBoard;

namespace Domain.Model.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponse>();
            CreateMap<ProjectRequest, Project>();
            CreateMap<TaskProjectRequest, TaskProject>();
            CreateMap<TaskProjectUpdateRequest, TaskProject>();
        }
    }
}
