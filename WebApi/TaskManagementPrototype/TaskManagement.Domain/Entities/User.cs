namespace TaskManagement.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public Guid RoleId { get; set; }

    public Role Role { get; set; } = null!;
}
