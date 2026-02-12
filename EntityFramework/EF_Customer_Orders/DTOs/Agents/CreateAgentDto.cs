using System.ComponentModel.DataAnnotations;

namespace EF_Customer_Orders.DTOs.Agents
{
    public class CreateAgentDto
    {
        [Required]
        [StringLength(6)]
        public string AgentCode { get; set; } = null!;

        [Required]
        [StringLength(40)]
        public string AgentName { get; set; } = null!;

        [Required]
        [StringLength(35)]
        public string WorkingArea { get; set; } = null!;

        [Range(0, 1)]
        public decimal Commission { get; set; }

        [Phone]
        public string? PhoneNo { get; set; }

        [Required]
        [StringLength(25)]
        public string Country { get; set; } = null!;
    }
}