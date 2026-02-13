using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task;

public class UpdateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public TaskStatus Status { get; set; }

    public TaskItemPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }
}
