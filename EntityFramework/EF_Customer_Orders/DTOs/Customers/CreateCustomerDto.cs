
using System.ComponentModel.DataAnnotations;

namespace EF_Customer_Orders.DTOs.Customers
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(6)]
        public string CustCode { get; set; } = null!;

        [Required]
        [StringLength(40)]
        public string CustName { get; set; } = null!;

        [Required]
        [StringLength(35)]
        public string CustCity { get; set; } = null!;

        [Required]
        [StringLength(35)]
        public string WorkingArea { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string CustCountry { get; set; } = null!;

        public int? Grade { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OpeningAmt { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ReceiveAmt { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PaymentAmt { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OutstandingAmt { get; set; }

        [Phone]
        public string? PhoneNo { get; set; }

        [Required]
        public Guid AgentId { get; set; }
    }
}