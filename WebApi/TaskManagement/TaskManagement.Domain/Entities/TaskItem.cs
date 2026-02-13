using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Backlog;
        public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;
        public DateTime? DueDate { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid ProjectId { get; set; }

        public virtual User? AssignedTo { get; set; }
        public virtual Project Project { get; set; } = null!;
    }
}