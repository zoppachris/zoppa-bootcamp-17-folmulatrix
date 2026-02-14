using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task
{
    public class TaskFilterDto
    {
        public TaskItemStatus? Status { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}