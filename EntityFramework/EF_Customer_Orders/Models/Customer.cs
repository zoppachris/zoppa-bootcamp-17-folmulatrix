using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Customer_Orders.Models
{
    [Table("CUSTOMER")]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("CUST_CODE")]
        [StringLength(6)]
        public string CustCode { get; set; } = null!;

        [Required]
        [Column("CUST_NAME")]
        [StringLength(40)]
        public string CustName { get; set; } = null!;

        [Column("CUST_CITY")]
        [StringLength(35)]
        public string CustCity { get; set; } = null!;

        [Column("WORKING_AREA")]
        [StringLength(35)]
        public string WorkingArea { get; set; } = null!;

        [Column("CUST_COUNTRY")]
        [StringLength(20)]
        public string CustCountry { get; set; } = null!;

        [Column("GRADE")]
        public int? Grade { get; set; }
        [Column("OPENING_AMT")]
        public decimal? OpeningAmt { get; set; }
        [Column("RECEIVE_AMT")]
        public decimal? ReceiveAmt { get; set; }
        [Column("PAYMENT_AMT")]
        public decimal? PaymentAmt { get; set; }
        [Column("OUTSTANDING_AMT")]
        public decimal? OutstandingAmt { get; set; }

        [Column("PHONE_NO")]
        [StringLength(17)]
        public string? PhoneNo { get; set; }

        // Foreign Key (surrogate)
        [Required]
        public Guid AgentId { get; set; }

        // Navigation
        public virtual Agent Agent { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}