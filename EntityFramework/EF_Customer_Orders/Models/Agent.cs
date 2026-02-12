using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Customer_Orders.Models
{
    [Table("AGENTS")]
    public class Agent
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("AGENT_CODE")]
        [StringLength(6)]
        public string AgentCode { get; set; } = null!;

        [Required]
        [Column("AGENT_NAME")]
        [StringLength(40)]
        public string AgentName { get; set; } = null!;

        [Column("WORKING_AREA")]
        [StringLength(35)]
        public string WorkingArea { get; set; } = null!;

        [Column("COMMISSION")]
        public decimal Commission { get; set; }

        [Column("PHONE_NO")]
        [StringLength(15)]
        public string? PhoneNo { get; set; }

        [Column("COUNTRY")]
        [StringLength(25)]
        public string Country { get; set; } = null!;

        // Navigation
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}