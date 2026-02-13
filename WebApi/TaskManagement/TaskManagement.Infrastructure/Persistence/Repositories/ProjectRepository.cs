using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task AssignMemberAsync(Guid projectId, Guid userId)
        {
            Project? project = await GetProjectWithMembersAsync(projectId);
            if (project == null) return;

            User? user = await _context.Users.FindAsync(userId);
            if (user != null && !project.Members.Contains(user))
            {
                project.Members.Add(user);
                await SaveChangesAsync();
            }
        }

        public async Task RemoveMemberAsync(Guid projectId, Guid userId)
        {
            Project? project = await GetProjectWithMembersAsync(projectId);
            if (project == null) return;

            User? user = project.Members.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                project.Members.Remove(user);
                await SaveChangesAsync();
            }
        }

        public async Task<Project?> GetProjectWithMembersAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> GetProjectWithTasksAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }
    }
}