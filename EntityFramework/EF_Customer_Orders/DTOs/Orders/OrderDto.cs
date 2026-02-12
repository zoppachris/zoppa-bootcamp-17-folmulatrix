namespace EF_Customer_Orders.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int OrdNum { get; set; }
        public decimal? OrdAmount { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public DateTime? OrdDate { get; set; }

        public string CustomerName { get; set; } = null!;

        public string AgentName { get; set; } = null!;
    }
}