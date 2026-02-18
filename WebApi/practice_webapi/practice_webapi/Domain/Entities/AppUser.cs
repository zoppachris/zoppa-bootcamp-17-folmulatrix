

using Microsoft.AspNetCore.Identity;

namespace practice_webapi.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>, IAuditableEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}