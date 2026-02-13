using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);

    Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId);

    Task<List<TaskItem>> FilterAsync(
        Guid? projectId,
        TaskStatus? status,
        TaskItemPriority? priority);

    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
