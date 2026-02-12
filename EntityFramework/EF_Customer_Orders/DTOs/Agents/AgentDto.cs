namespace EF_Customer_Orders.DTOs.Agents
{
    public class AgentDto
    {
        public Guid Id { get; set; }
        public string AgentCode { get; set; } = null!;
        public string AgentName { get; set; } = null!;
        public string WorkingArea { get; set; } = null!;
        public decimal Commission { get; set; }
        public string? PhoneNo { get; set; }
        public string Country { get; set; } = null!;
    }
}