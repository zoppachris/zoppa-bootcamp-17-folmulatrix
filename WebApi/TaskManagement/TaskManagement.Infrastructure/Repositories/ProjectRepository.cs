using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Project?> GetProjectWithMembersAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Where(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }
    }
}