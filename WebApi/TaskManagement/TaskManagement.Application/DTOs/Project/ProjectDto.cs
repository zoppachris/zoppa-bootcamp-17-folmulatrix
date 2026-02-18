using TaskManagement.Application.DTOs.Task;
namespace TaskManagement.Application.DTOs.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public List<ProjectMemberDto> Members { get; set; } = new();
        public List<TaskItemDto>? Tasks { get; set; }
    }
}