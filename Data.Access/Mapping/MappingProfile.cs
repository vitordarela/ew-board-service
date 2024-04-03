using AutoMapper;
using Domain.Model.DTO.Comment;
using Domain.Model.DTO.Project;
using Domain.Model.DTO.TaskBoard;
using Domain.Model.DTO.User;

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
            CreateMap<CommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<UserRequest, User>();
        }
    }
}
