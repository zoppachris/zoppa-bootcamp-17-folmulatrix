using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IList<string>> GetRolesAsync(User user);
    }
}