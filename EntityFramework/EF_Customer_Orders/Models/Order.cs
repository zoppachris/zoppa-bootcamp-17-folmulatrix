using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Customer_Orders.Models
{
    [Table("ORDERS")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("ORD_NUM")]
        public int OrdNum { get; set; }

        [Column("ORD_AMOUNT")]
        public decimal? OrdAmount { get; set; }

        [Column("ADVANCE_AMOUNT")]
        public decimal? AdvanceAmount { get; set; }

        [Column("ORD_DATE")]
        public DateTime? OrdDate { get; set; }

        // Foreign Keys (surrogate)
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid AgentId { get; set; }


        [Column("ORD_DESCRIPTION")]
        [StringLength(60)]
        public string OrdDescription { get; set; } = string.Empty;

        // Navigation
        public Customer Customer { get; set; } = new();
        public Agent Agent { get; set; } = new();
    }
}