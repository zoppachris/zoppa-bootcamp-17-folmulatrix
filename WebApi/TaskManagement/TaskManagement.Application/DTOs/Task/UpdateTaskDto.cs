using TaskManagement.Domain.Enums;
namespace TaskManagement.Application.DTOs.Task
{
    public class UpdateTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskItemStatus Status { get; set; }
        public TaskItemPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? AssignedUserId { get; set; }
    }
}