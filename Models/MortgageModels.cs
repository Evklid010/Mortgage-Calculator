using System;

namespace WebApplication1.Models
{
    public class MortgageRequest
    {
        public decimal PropertyPrice { get; set; }
        public decimal DownPayment { get; set; }
        public double InterestRate { get; set; }
        public int Years { get; set; }
        public string PaymentType { get; set; } = "annuity";
    }

    public class MortgageResult
    {
        public decimal MonthlyPayment { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal Overpayment { get; set; }
    }

    public class MortgageCalculation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public MortgageRequest Request { get; set; }
        public MortgageResult Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}