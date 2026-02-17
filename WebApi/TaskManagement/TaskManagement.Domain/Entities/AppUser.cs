using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
    }
}