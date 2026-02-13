
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task AssignMemberAsync(Guid projectId, Guid userId);
        Task RemoveMemberAsync(Guid projectId, Guid userId);
        Task<Project?> GetProjectWithMembersAsync(Guid projectId);
        Task<Project?> GetProjectWithTasksAsync(Guid projectId);
    }
}