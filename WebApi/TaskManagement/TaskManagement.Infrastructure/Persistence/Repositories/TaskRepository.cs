using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : RepositoryBase<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public async Task AssignUserAsync(Guid taskId, Guid userId)
        {
            TaskItem? task = await GetByIdAsync(taskId);
            if (task != null)
            {
                task.AssignedToId = userId;
                Update(task);
                await SaveChangesAsync();
            }
        }

        public IQueryable<TaskItem> GetTasksByStatus(TaskItemStatus status)
        {
            return Find(t => t.Status == status).Include(t => t.Project).Include(t => t.AssignedTo);
        }

        public IQueryable<TaskItem> GetTasksByProject(Guid projectId)
        {
            return Find(t => t.ProjectId == projectId).Include(t => t.Project).Include(t => t.AssignedTo);
        }

        public IQueryable<TaskItem> GetTasksByAssignedUser(Guid userId)
        {
            return Find(t => t.AssignedToId == userId).Include(t => t.Project).Include(t => t.AssignedTo);
        }

        public IQueryable<TaskItem> GetTasksByDueDate(DateTime dueDate)
        {
            return Find(t => t.DueDate == dueDate).Include(t => t.Project).Include(t => t.AssignedTo);
            // Untuk range (misal overdue): Di services, e.g., .Where(t => t.DueDate < DateTime.UtcNow)
        }
    }
}