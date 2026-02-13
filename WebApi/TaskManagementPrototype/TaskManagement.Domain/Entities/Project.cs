namespace TaskManagement.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public Guid OwnerId { get; set; }

    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
}
