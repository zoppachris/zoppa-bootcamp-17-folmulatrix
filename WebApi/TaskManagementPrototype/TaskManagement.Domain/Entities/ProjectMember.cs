namespace TaskManagement.Domain.Entities;

public class ProjectMember
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }

    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
}
