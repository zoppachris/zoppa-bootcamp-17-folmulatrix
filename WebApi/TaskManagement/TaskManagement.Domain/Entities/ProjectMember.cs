namespace TaskManagement.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}