using AutoMapper;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.MappingProfiles
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TaskItem, TaskDto>();
        }
    }
}