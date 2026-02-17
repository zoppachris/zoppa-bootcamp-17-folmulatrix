using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetProjectWithMembersAsync(Guid projectId);
        Task<IEnumerable<Project>> GetUserProjectsAsync(Guid userId);
    }
}