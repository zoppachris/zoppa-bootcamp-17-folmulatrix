
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task AssignUserAsync(Guid taskId, Guid userId);
        IQueryable<TaskItem> GetTasksByStatus(TaskItemStatus status);
        IQueryable<TaskItem> GetTasksByProject(Guid projectId);
        IQueryable<TaskItem> GetTasksByAssignedUser(Guid userId);
        IQueryable<TaskItem> GetTasksByDueDate(DateTime dueDate);
    }
}