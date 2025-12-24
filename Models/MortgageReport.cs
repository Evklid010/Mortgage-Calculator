using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MortgageReport
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Данные калькулятора
        public decimal PropertyPrice { get; set; }
        public decimal DownPayment { get; set; }
        public double InterestRate { get; set; }
        public int Years { get; set; }
        public string PaymentType { get; set; } // "annuity" или "diff"

        // Результаты расчёта
        public decimal MonthlyPayment { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal Overpayment { get; set; }

        // Для удобного отображения (не сохраняется в БД)
        [NotMapped]
        public string ReportName => $"Отчёт от {CreatedAt:dd.MM.yyyy HH:mm}";
    }
}