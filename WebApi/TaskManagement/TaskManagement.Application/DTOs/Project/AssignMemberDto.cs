namespace TaskManagement.Application.DTOs.Project
{
    public class AssignMemberDto
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}