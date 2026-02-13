namespace TaskManagement.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? OwnerId { get; set; }

        public virtual User? Owner { get; set; }
        public virtual ICollection<User> Members { get; set; } = new List<User>();
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}