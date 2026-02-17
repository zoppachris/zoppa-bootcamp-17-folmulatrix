using TaskManagement.Application.DTOs.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task
{
    public class TaskFilterDto : FilterDto
    {
        public TaskItemStatus? Status { get; set; }
        public TaskItemPriority? Priority { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}