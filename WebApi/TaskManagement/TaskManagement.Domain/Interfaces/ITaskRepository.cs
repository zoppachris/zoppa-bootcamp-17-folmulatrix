using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetTasksByProjectAsync(Guid projectId);
        Task<IEnumerable<TaskItem>> GetTasksByAssignedUserAsync(Guid userId);
        Task<TaskItem?> GetTaskWithDetailsAsync(Guid taskId);
    }
}