using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.DTOs.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? OwnerId { get; set; }
        public IEnumerable<UserDto> Members { get; set; } = new List<UserDto>();
        public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}