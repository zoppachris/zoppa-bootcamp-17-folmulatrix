using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Guid ProjectId { get; set; }

    public TaskItemPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }
}
