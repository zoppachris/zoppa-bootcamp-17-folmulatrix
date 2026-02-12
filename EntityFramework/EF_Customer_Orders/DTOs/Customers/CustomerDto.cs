using EF_Customer_Orders.DTOs.Agents;

namespace EF_Customer_Orders.DTOs.Customers
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string CustCode { get; set; } = null!;
        public string CustName { get; set; } = null!;
        public string CustCity { get; set; } = null!;
        public string WorkingArea { get; set; } = null!;
        public string CustCountry { get; set; } = null!;
        public int? Grade { get; set; }
        public decimal OpeningAmt { get; set; }
        public decimal ReceiveAmt { get; set; }
        public decimal PaymentAmt { get; set; }
        public decimal OutstandingAmt { get; set; }
        public string? PhoneNo { get; set; }

        public AgentDto Agent { get; set; } = null!;
    }

}