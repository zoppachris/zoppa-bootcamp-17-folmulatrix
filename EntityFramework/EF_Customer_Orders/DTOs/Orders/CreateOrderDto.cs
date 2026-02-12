using System.ComponentModel.DataAnnotations;

namespace EF_Customer_Orders.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public int OrdNum { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OrdAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AdvanceAmount { get; set; }

        [Required]
        public DateTime OrdDate { get; set; }

        public string? OrdDescription { get; set; }

        // FK
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid AgentId { get; set; }
    }
}