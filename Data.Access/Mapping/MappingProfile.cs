using AutoMapper;
using Domain.Model.DTO.Project;
using Domain.Model.DTO.TaskBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponse>();
            CreateMap<ProjectRequest, Project>();
            CreateMap<TaskProjectRequest, TaskProject>();
        }
    }
}
