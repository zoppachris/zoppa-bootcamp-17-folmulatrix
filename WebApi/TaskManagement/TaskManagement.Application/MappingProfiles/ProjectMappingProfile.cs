using AutoMapper;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Project, ProjectDto>();
        }
    }
}